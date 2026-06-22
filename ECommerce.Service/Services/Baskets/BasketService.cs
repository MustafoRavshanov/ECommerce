using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Helper;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerce.Service.Services.Baskets;

public class BasketService(ApplicationDbContext applicationDbContext, IMapper mapper) : IBasketService
{
    public async Task<ResponseModel<BasketDto>> AddBasketAsync(BasketCreateDto basketCreateDto, int customerId)
    {
        var existingItem = await applicationDbContext.Baskets.FirstOrDefaultAsync(x => x.CustomerId == customerId && x.ProductId == basketCreateDto.ProductId);

        var product = await applicationDbContext.Products.FirstOrDefaultAsync(x => x.Id == basketCreateDto.ProductId);

        if (existingItem != null)
        {
            if(product.Stock < basketCreateDto.Quantity)
                return ResponseModel<BasketDto>.Fail("Not enough stock for the product", HttpStatusCode.BadRequest);

            existingItem.Quantity += basketCreateDto.Quantity;
            await applicationDbContext.SaveChangesAsync();
           
            return await GetMyBasketAsync(customerId);
        }
        var entity = mapper.Map<Basket>(basketCreateDto);
        entity.CustomerId = customerId;

        await applicationDbContext.Baskets.AddAsync(entity);

        if(product.Stock < basketCreateDto.Quantity)
            return ResponseModel<BasketDto>.Fail("Not enough stock for the product", HttpStatusCode.BadRequest);

        product.Stock -= basketCreateDto.Quantity;

        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<BasketDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        return await GetMyBasketAsync(customerId);
    }

    public async Task<ResponseModel<BasketDto>> GetMyBasketAsync( int customerId)
    {
        var items = await applicationDbContext.Baskets.Include(x => x.Product).Where(x => x.CustomerId == customerId).ToListAsync();

        if (items is null)
            return ResponseModel<BasketDto>.Fail("Basket not found", HttpStatusCode.NotFound);

        var basketDto = new BasketDto
        {
            Items = mapper.Map<List<BasketItemDto>>(items),
        };

        return ResponseModel<BasketDto>.Success(basketDto, "Basket retrieved successfully", HttpStatusCode.OK);
    }


    public async Task<ResponseModel<BasketDto>> UpdateBasketAsync(BasketUpdateDto basketUpdateDto, int basketId, int customerId)
    {
        var entity = await applicationDbContext.Baskets.FirstOrDefaultAsync(x => x.Id == basketId && x.CustomerId == customerId);

        if (entity is null)
            return ResponseModel<BasketDto>.Fail("Basket item not found", HttpStatusCode.NotFound);

        if(basketUpdateDto.Quantity is null || basketUpdateDto.Quantity < 1)
            return ResponseModel<BasketDto>.Fail("Quantity is required and must be at least 1", HttpStatusCode.BadRequest);

        var difference = basketUpdateDto.Quantity - entity.Quantity;

        if (entity.Product is null)
            return ResponseModel<BasketDto>.Fail("Product not found", HttpStatusCode.NotFound);

        if (difference > 0 && entity.Product.Stock < difference)
            return ResponseModel<BasketDto>.Fail("Not enough stock for the product", HttpStatusCode.BadRequest);

        entity.Product.Stock -= (int)difference;
        entity.Quantity = (int)basketUpdateDto.Quantity;

        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<BasketDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        return await GetMyBasketAsync(customerId);
    }

    public async Task<ResponseModel<bool>> RemoveFromBasketAsync(int basketId, int customerId)
    {
        var entity = await applicationDbContext.Baskets.FirstOrDefaultAsync(x => x.Id == basketId && x.CustomerId == customerId);

        if (entity is null)
            return ResponseModel<bool>.Fail("Basket item not found", HttpStatusCode.NotFound);

        var product = await applicationDbContext.Products.FirstOrDefaultAsync(x => x.Id == entity.ProductId);
        product.Stock += entity.Quantity;

        applicationDbContext.Baskets.Remove(entity);
        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<bool>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        return ResponseModel<bool>.Success(true, "Basket item deleted successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<bool>> ClearBasketAsync(int customerId)
    {
        var items = await applicationDbContext.Baskets.Where(x => x.CustomerId == customerId).ToListAsync();

        if (!items.Any())
            return ResponseModel<bool>.Fail("Basket is already empty", HttpStatusCode.BadRequest);

        applicationDbContext.Baskets.RemoveRange(items);
        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<bool>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        return ResponseModel<bool>.Success(true, "Basket cleared successfully", HttpStatusCode.OK);
    }
}

