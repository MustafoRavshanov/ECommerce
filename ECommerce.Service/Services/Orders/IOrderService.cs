using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;

namespace ECommerce.Service.Services.Orders
{
    public interface IOrderService
    {
        Task<ResponseModel<OrderDto>> AddOrderAsync(OrderCreateDto createDto, int customerId);
        Task<TableResponse<List<OrderDto>>> GetMyOrdersAsync(TableOptions tableOptions, int customerId);
        Task<ResponseModel<OrderDto>> GetOrderByIdAsync(int orderId, int customerId);
        Task<ResponseModel<OrderDto>> UpdateOrderAsync(OrderUpdateDto updateDto, int orderId, int customerId);
        
        Task<ResponseModel<bool>> DeleteOrderAsync(int orderId, int customerId);
    }
}
