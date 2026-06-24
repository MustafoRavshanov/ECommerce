using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;

namespace ECommerce.Service.Services.Baskets;

public interface IBasketService
{
    Task<ResponseModel<BasketDto>>AddBasketAsync(BasketCreateDto basketCreateDto, int customerId);
    Task<ResponseModel<BasketDto>>GetMyBasketAsync(int customerId);
    Task<ResponseModel<BasketItemDto>> GetBasketByIdAsync(int customerId, int basketId);
    Task<ResponseModel<BasketDto>>UpdateBasketAsync(BasketUpdateDto basketUpdateDto, int basketId, int customerId);
    Task<ResponseModel<bool>> RemoveFromBasketAsync(int basketId, int customerId);
    Task<ResponseModel<bool>> ClearBasketAsync(int customerId);
}
