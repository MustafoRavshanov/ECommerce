using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerce.Service.Services.OrderDetails;

public class OrderDetailService(ApplicationDbContext applicationDbContext, IMapper mapper) : IOrderDetailService
{
    public async Task<TableResponse<List<OrderDetailDto>>> GetAllOrderDetailsAsync(TableOptions options)
    {
        var orderDetails = applicationDbContext.OrderDetails.Include(x => x.Order).Include(x=>x.Product).AsQueryable();

        var count=await orderDetails.CountAsync();

        var entity= await orderDetails
            .Skip(options.First)
            .Take(options.Rows)
            .ToListAsync();

        var dtos=mapper.Map<List<OrderDetailDto>>(entity);

        return new TableResponse<List<OrderDetailDto>>() { Total = count, Items = dtos };
    }

    public async Task<ResponseModel<OrderDetailDto>> GetOrderDetailByOrderIdAsync(int orderId, int customerId)
    {
        var orderDetail = await applicationDbContext.OrderDetails
            .Include(x=>x.Product)
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == orderId && x.Order.CustomerId == customerId);

        if (orderDetail is null)
            return ResponseModel<OrderDetailDto>.Fail("Order not found", HttpStatusCode.NotFound);

        var dto= mapper.Map<OrderDetailDto>(orderDetail);

        return ResponseModel<OrderDetailDto>.Success(dto, "Order retrieved successfully", HttpStatusCode.OK);
    }
}
