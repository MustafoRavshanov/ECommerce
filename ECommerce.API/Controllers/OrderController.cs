using ECommerce.API.Extentions;
using ECommerce.API.Filters;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;
[ApiController]
[Route("api/order")]
[Authorize]
public class OrderController(IOrderService orderService): ControllerBase
{
    [HttpPost("create")]
    public async Task<ResponseModel<OrderDto>> CreateAsync(OrderCreateDto dto)
    { 
        var customerId=User.GetUserId();
        return await orderService.AddOrderAsync(dto, customerId);
    }

    [HttpGet("get-my-orders")]
    public async Task<TableResponse<List<OrderDto>>> GetAllAsync([FromQuery] TableOptions options)
    { 
        var customerId=User.GetUserId();
        return await orderService.GetMyOrdersAsync(options,customerId);
    }

    [HttpGet("get-all-full/{customerId}")]
    [HasPermission(Permission.OrderManage)]
    public async Task<TableResponse<List<OrderFullInformationDto>>> GetAllFullAsync([FromQuery] TableOptions options, [FromRoute] int customerId) =>
        await orderService.GetAllOrdersFullAsync(options, customerId);

    [HttpGet("get-by-id/{id}")]
    public async Task<ResponseModel<OrderDto>> GetByIdAsync([FromRoute] int id)
    { 
        var customerId = User.GetUserId();
        return await orderService.GetOrderByIdAsync(id, customerId);
    }

    [HttpGet("get-full-by-id/{id}/{customerId}")]
    [HasPermission(Permission.OrderManage)]
    public async Task<ResponseModel<OrderFullInformationDto>> GetFullByIdAsync([FromRoute] int id, [FromRoute] int customerId) =>
        await orderService.GetOrderFullByIdAsync(id, customerId);

    [HttpPut("update/{id}")]
    public async Task<ResponseModel<OrderDto>> UpdateAsync(OrderUpdateDto dto, [FromRoute] int id)
    { 
        var customerId=User.GetUserId();
        return await orderService.UpdateOrderAsync(dto, id, customerId);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int id)
    { 
        var customerId=User.GetUserId() ;
        return await orderService.DeleteOrderAsync(id, customerId);
    }
}
