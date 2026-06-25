using ECommerce.API.Extentions;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Baskets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;
[ApiController]
[Route("api/basket")]
[Authorize]
public class BasketController(IBasketService basketService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<ResponseModel<BasketDto>> CreateAsync(BasketCreateDto dto) 
    {
        var customerId = User.GetUserId();
        return await basketService.AddBasketAsync(dto, customerId);
    }

    [HttpGet("get-by-customer-id")]
    public async Task<ResponseModel<BasketDto>> GetByCustomerIdAsync()
    { 
        var customerId= User.GetUserId();
        return await basketService.GetMyBasketAsync(customerId);
    }

    [HttpGet("get-basket-by-id/{basketId}")]
    public async Task<ResponseModel<BasketItemDto>> GetBasketItemById([FromRoute] int basketId)
    { 
        var customerId=User.GetUserId();
        return await basketService.GetBasketByIdAsync(customerId, basketId);
    }

    [HttpPut("update/{basketId}")]
    public async Task<ResponseModel<BasketDto>> UpdateAsync([FromBody] BasketUpdateDto dto, [FromRoute] int basketId)
    { 
        var customerId= User.GetUserId() ;
        return await basketService.UpdateBasketAsync(dto, basketId, customerId);
    }

    [HttpDelete("delete/{basketId}")]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int basketId)
    {
        var customerId = User.GetUserId();
        return await basketService.RemoveFromBasketAsync(basketId, customerId);
    }


    [HttpDelete("clear")]
    public async Task<ResponseModel<bool>> ClearAsync()
    { 
        var customerId = User.GetUserId();
        return await basketService.ClearBasketAsync(customerId);
    }
}
