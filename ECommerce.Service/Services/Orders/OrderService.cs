using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Helper;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerce.Service.Services.Orders
{
    public class OrderService(ApplicationDbContext applicationDbContext, IMapper mapper) : IOrderService
    {
        public async Task<ResponseModel<OrderDto>> AddOrderAsync(OrderCreateDto createDto, int customerId)
        {
            var basketItems = await applicationDbContext.Baskets
                .Include(x => x.Product)
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();

            if(!basketItems.Any())
                return ResponseModel<OrderDto>.Fail("Basket is empty", HttpStatusCode.BadRequest);

            var customer = await applicationDbContext.Customers.FirstOrDefaultAsync(x => x.Id == customerId);

            var district = await applicationDbContext.Districts.FirstOrDefaultAsync(x => x.Id == customer.DistrictId);

            if(district is null)
                return ResponseModel<OrderDto>.Fail("District not found", HttpStatusCode.NotFound);

            var totalPrice = basketItems.Sum(x => x.Product.Price * x.Quantity);

            var entity = new Order
            {
                CustomerId = customerId,
                DistrictId = customer.DistrictId ?? 0,
                Address = customer.Address,
                PaymentType = createDto.PaymentType,
                TotalPrice = totalPrice,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await applicationDbContext.Orders.AddAsync(entity);
            await applicationDbContext.SaveChangesAsync();

            var orderDetails = basketItems.Select(x => new OrderDetail
            {
                OrderId = entity.Id,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                UnitPrice = x.Product.Price,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            await applicationDbContext.OrderDetails.AddRangeAsync(orderDetails);

            foreach(var item in basketItems)
            {
                item.Product.Stock -= item.Quantity;
            }

            applicationDbContext.Baskets.RemoveRange(basketItems);

            var result = await applicationDbContext.SaveChangesAsync();

            if(result < 1)
                return ResponseModel<OrderDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

            var orderDto = mapper.Map<OrderDto>(entity);

            return ResponseModel<OrderDto>.Success(orderDto, "Order created successfully", HttpStatusCode.Created);
        }

        public async Task<TableResponse<List<OrderDto>>> GetMyOrdersAsync(TableOptions tableOptions, int customerId)
        {
           var entities = applicationDbContext.Orders.Where(x => x.CustomerId == customerId).AsQueryable();
           var total = await entities.CountAsync();
           var orders = await entities
                .Skip(tableOptions.First)
                .Take(tableOptions.Rows)
                .ToListAsync();

            var orderDtos = mapper.Map<List<OrderDto>>(orders);

            return new TableResponse<List<OrderDto>> { Total = total, Items = orderDtos };
        }

        public async Task<TableResponse<List<OrderFullInformationDto>>> GetAllOrdersFullAsync(TableOptions tableOptions, int customerId)
        {
            var entities = applicationDbContext.Orders
                .Include(o => o.District)
                .ThenInclude(d => d.Region)
                .Include(o => o.Customer)
                .ThenInclude(c => c.User)
                .Include(o => o.OrderDetails)
                .Where(x => x.CustomerId == customerId).AsQueryable();

            var total = await entities.CountAsync();
            var orders = await entities
                 .Skip(tableOptions.First)
                 .Take(tableOptions.Rows)
                 .ToListAsync();

            var orderDtos = mapper.Map<List<OrderFullInformationDto>>(orders);

            return new TableResponse<List<OrderFullInformationDto>> { Total = total, Items = orderDtos };
        }
        public async Task<ResponseModel<OrderDto>> GetOrderByIdAsync(int orderId, int customerId)
        {
            var entity = await applicationDbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.CustomerId == customerId);

            if (entity is null)
                return ResponseModel<OrderDto>.Fail("Order not found", HttpStatusCode.NotFound);

            var orderDto = mapper.Map<OrderDto>(entity);

            return ResponseModel<OrderDto>.Success(orderDto, "Order retrieved successfully", HttpStatusCode.OK);
        }

        public async Task<ResponseModel<OrderFullInformationDto>> GetOrderFullByIdAsync(int orderId, int customerId)
        {
            var entity = await applicationDbContext.Orders
                .Include(o => o.District)
                .ThenInclude(d => d.Region)
                .Include(o => o.Customer)
                .ThenInclude(c => c.User)
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(x => x.Id == orderId && x.CustomerId==customerId);

            if (entity is null)
                return ResponseModel<OrderFullInformationDto>.Fail("Order not found", HttpStatusCode.NotFound);

            var orderDto = mapper.Map<OrderFullInformationDto>(entity);

            return ResponseModel<OrderFullInformationDto>.Success(orderDto, "Order retrieved successfully", HttpStatusCode.OK);
        }
        public async Task<ResponseModel<OrderDto>> UpdateOrderAsync(OrderUpdateDto updateDto, int orderId, int customerId)
        {
            var entity = await applicationDbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.CustomerId == customerId);

            if (entity is null)
                return ResponseModel<OrderDto>.Fail("Order not found", HttpStatusCode.NotFound);

           if(entity.Status== OrderStatus.Cancelled)
                return ResponseModel<OrderDto>.Fail("Cancelled orders cannot be updated", HttpStatusCode.BadRequest);

           if(entity.Status == OrderStatus.Delivered)
                return ResponseModel<OrderDto>.Fail("Delivered orders cannot be updated", HttpStatusCode.BadRequest);

            mapper.Map(updateDto, entity);
            var result = await applicationDbContext.SaveChangesAsync();

            if (result < 1)
                return ResponseModel<OrderDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

            var orderDto = mapper.Map<OrderDto>(entity);

            return ResponseModel<OrderDto>.Success(orderDto, "Order updated successfully", HttpStatusCode.OK);
        }

        public async Task<ResponseModel<bool>> DeleteOrderAsync(int orderId, int customerId)
        {
            var entity = await applicationDbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.CustomerId == customerId);

            if(entity is null)
                return ResponseModel<bool>.Fail("Order not found", HttpStatusCode.NotFound);

            if(entity.Status != OrderStatus.Pending)
                return ResponseModel<bool>.Fail("Only pending orders can be deleted", HttpStatusCode.BadRequest);

            entity.Status = OrderStatus.Cancelled;
            entity.UpdatedAt = DateTime.UtcNow;

            var result = await applicationDbContext.SaveChangesAsync();

            if(result < 1)
                return ResponseModel<bool>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

            return ResponseModel<bool>.Success(true, "Order deleted successfully", HttpStatusCode.OK);
        }
    }
}
