using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Baskets;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;
[ApiController]
[Route("api/basket")]
public class BasketController(IBasketService basketService) : ControllerBase
{
    [HttpPost("create/{customerId}")]
    public async Task<ResponseModel<BasketDto>> CreateAsync(BasketCreateDto dto, [FromRoute] int customerId) =>
        await basketService.AddBasketAsync(dto, customerId);

    [HttpGet("get-by-customer-id/{customerId}")]
    public async Task<ResponseModel<BasketDto>> GetByCustomerIdAsync([FromRoute] int customerId) =>
        await basketService.GetMyBasketAsync(customerId);

    [HttpGet("get-basket-by-id/{basketId}/{customerId}")]
    public async Task<ResponseModel<BasketItemDto>> GetBasketItemById([FromRoute] int basketId, [FromRoute] int customerId) =>
        await basketService.GetBasketByIdAsync(basketId, customerId);

    [HttpPut("update/{basketId}/{customerId}")]
    public async Task<ResponseModel<BasketDto>> UpdateAsync([FromBody]BasketUpdateDto dto, [FromRoute] int basketId, [FromRoute] int customerId) =>
        await basketService.UpdateBasketAsync(dto, basketId, customerId);

    [HttpDelete("delete/{basketId}/{customerId}")]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int basketId, [FromRoute] int customerId) =>
        await basketService.RemoveFromBasketAsync(basketId, customerId);


    [HttpDelete("clear/{customerId}")]
    public async Task<ResponseModel<bool>> ClearAsync([FromRoute] int customerId) =>
        await basketService.ClearBasketAsync(customerId);
}
