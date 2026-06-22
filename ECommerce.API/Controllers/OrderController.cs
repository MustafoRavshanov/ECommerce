using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Orders;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;
[ApiController]
[Route("api/order")]
public class OrderController(IOrderService orderService): ControllerBase
{
    [HttpPost("create/{customerId}")]
    public async Task<ResponseModel<OrderDto>> CreateAsync(OrderCreateDto dto, [FromRoute]int customerId) =>
        await orderService.AddOrderAsync(dto, customerId);

    [HttpGet("get-all/{customerId}")]
    public async Task<TableResponse<List<OrderDto>>> GetAllAsync([FromQuery] TableOptions options, [FromRoute]int customerId) =>
        await orderService.GetMyOrdersAsync(options, customerId);

    [HttpGet("get-by-id/{id}/{customerId}")]
    public async Task<ResponseModel<OrderDto>> GetByIdAsync([FromRoute] int id, [FromRoute]int customerId) =>
        await orderService.GetOrderByIdAsync(id, customerId);

    [HttpPut("update/{id}/{customerId}")]
    public async Task<ResponseModel<OrderDto>> UpdateAsync(OrderUpdateDto dto, [FromRoute] int id, [FromRoute]int customerId) =>
        await orderService.UpdateOrderAsync(dto, id, customerId);

    [HttpDelete("delete/{id}/{customerId}")]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int id, [FromRoute]int customerId) =>
        await orderService.DeleteOrderAsync(id, customerId);
}
