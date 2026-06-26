using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;

namespace ECommerce.Service.Services.OrderDetails;

public interface IOrderDetailService
{
    Task<ResponseModel<OrderDetailDto>> GetOrderDetailByOrderIdAsync(int orderId, int customerId);
    Task<TableResponse<List<OrderDetailDto>>> GetAllOrderDetailsAsync(TableOptions options);
}
