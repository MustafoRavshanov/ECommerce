using ECommerce.API.Extentions;
using ECommerce.API.Filters;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/order")]
[Authorize]
public class OrderController(IOrderService orderService): BaseController
{
    [HttpPost("create")]
    public async Task<ResponseModel<OrderDto>> CreateAsync(OrderCreateDto dto) =>
        await orderService.AddOrderAsync(dto, CurrentUserId);

    [HttpGet("get-my-orders")]
    public async Task<TableResponse<List<OrderDto>>> GetAllAsync([FromQuery] TableOptions options) =>
        await orderService.GetMyOrdersAsync(options,CurrentUserId);

    [HttpGet("get-all-full/{customerId}")]
    [HasPermission(Permission.OrdersView)]
    public async Task<TableResponse<List<OrderFullInformationDto>>> GetAllFullAsync([FromQuery] TableOptions options, [FromRoute] int customerId) =>
        await orderService.GetAllOrdersFullAsync(options, customerId);

    [HttpGet("get-by-id/{id}")]
    public async Task<ResponseModel<OrderDto>> GetByIdAsync([FromRoute] int id) =>
        await orderService.GetOrderByIdAsync(id, CurrentUserId);

    [HttpGet("get-full-by-id/{id}/{customerId}")]
    [HasPermission(Permission.OrdersView)]
    public async Task<ResponseModel<OrderFullInformationDto>> GetFullByIdAsync([FromRoute] int id, [FromRoute] int customerId) =>
        await orderService.GetOrderFullByIdAsync(id, customerId);

    [HttpPut("update/{id}")]
    [HasPermission(Permission.OrdersEdit)]
    public async Task<ResponseModel<OrderDto>> UpdateAsync(OrderUpdateDto dto, [FromRoute] int id) =>
        await orderService.UpdateOrderAsync(dto, id, CurrentUserId);

    [HttpDelete("delete/{id}")]
    [HasPermission(Permission.OrdersCancel)]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int id) =>
        await orderService.DeleteOrderAsync(id, CurrentUserId);
}
