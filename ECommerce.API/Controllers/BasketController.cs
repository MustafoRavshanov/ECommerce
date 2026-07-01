using ECommerce.API.Extentions;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Baskets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/basket")]
[Authorize]
public class BasketController(IBasketService basketService) : BaseController
{
    [HttpPost("create")]
    public async Task<ResponseModel<BasketDto>> CreateAsync(BasketCreateDto dto) =>
        await basketService.AddBasketAsync(dto, CurrentUserId);

    [HttpGet("get-by-customer-id")]
    public async Task<ResponseModel<BasketDto>> GetByCustomerIdAsync() =>
        await basketService.GetMyBasketAsync(CurrentUserId);

    [HttpGet("get-basket-by-id/{basketId}")]
    public async Task<ResponseModel<BasketItemDto>> GetBasketItemById([FromRoute] int basketId) =>
        await basketService.GetBasketByIdAsync(CurrentUserId, basketId);

    [HttpPut("update/{basketId}")]
    public async Task<ResponseModel<BasketDto>> UpdateAsync([FromBody] BasketUpdateDto dto, [FromRoute] int basketId) =>
        await basketService.UpdateBasketAsync(dto, basketId, CurrentUserId);

    [HttpDelete("delete/{basketId}")]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int basketId) =>
        await basketService.RemoveFromBasketAsync(basketId, CurrentUserId);


    [HttpDelete("clear")]
    public async Task<ResponseModel<bool>> ClearAsync() =>
        await basketService.ClearBasketAsync(CurrentUserId);
}
