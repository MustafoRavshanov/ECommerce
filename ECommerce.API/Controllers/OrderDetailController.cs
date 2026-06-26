using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.OrderDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/order-detail")]
[Authorize]
public class OrderDetailController(IOrderDetailService orderDetailService):BaseController
{
    [HttpGet("get-by-id/{orderId}")]
    public async Task<ResponseModel<OrderDetailDto>> GetByIdAsync([FromRoute] int orderId) =>
        await orderDetailService.GetOrderDetailByOrderIdAsync(orderId, CurrentUserId);

    [HttpGet("get-all")]
    public async Task<TableResponse<List<OrderDetailDto>>> GetAllAsync([FromQuery] TableOptions options) =>
        await orderDetailService.GetAllOrderDetailsAsync(options);
}
