using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Helper;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerce.Service.Services.Products;

public class ProductService(ApplicationDbContext applicationDbContext, IMapper mapper) : IProductService
{
    public async Task<ResponseModel<ProductFullDto>> CreateProductAsync(ProductCreateDto createDto)
    {
        var product = await applicationDbContext.Products
            .FirstOrDefaultAsync(x => x.NameEn == createDto.NameEn || x.NameUz == createDto.NameUz);

        if (product != null)
            return ResponseModel<ProductFullDto>.Fail("This product name already exists", HttpStatusCode.Conflict);

        var category = await applicationDbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == createDto.CategoryId);

        var entity = mapper.Map<Product>(createDto);
        entity.Category = category;

        await applicationDbContext.Products.AddAsync(entity);
        var result = await applicationDbContext.SaveChangesAsync();

        if(result < 1)
            return ResponseModel<ProductFullDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var productDto = mapper.Map<ProductFullDto>(entity);

        return ResponseModel<ProductFullDto>.Success(productDto, "Product added successfully", HttpStatusCode.Created);
    }

    public async Task<ResponseModel<bool>> DeleteProductAsync(int productId)
    {
        var entity = await applicationDbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (entity is null)
            return ResponseModel<bool>.Fail("Product not found", HttpStatusCode.NotFound);

        applicationDbContext.Products.Remove(entity);
        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<bool>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        return ResponseModel<bool>.Success(true, "Product deleted successfully", HttpStatusCode.OK);
    }

    public async Task<TableResponse<List<ProductFullDto>>> GetAllProductsAsync(TableOptions tableOptions)
    {
        var entities = applicationDbContext.Products.Include(p => p.Category).AsQueryable();
        var count = await entities.CountAsync();

        var products = await entities
            .Skip(tableOptions.First)
            .Take(tableOptions.Rows)
            .ToListAsync();

        var productDtos = mapper.Map<List<ProductFullDto>>(products);
       
        return new TableResponse<List<ProductFullDto>> { Total = count, Items = productDtos };
    }

    public async Task<TableResponse<List<ProductFullInformationDto>>> GetAllProductFullAsync(TableOptions tableOptions)
    {
        var entities = applicationDbContext.Products.Include(p => p.Category).AsQueryable();
        var count = await entities.CountAsync();

        var products = await entities
            .Skip(tableOptions.First)
            .Take(tableOptions.Rows)
            .ToListAsync();

        var productDtos = mapper.Map<List<ProductFullInformationDto>>(products);

        return new TableResponse<List<ProductFullInformationDto>> { Total = count, Items = productDtos };
    }

    public async Task<ResponseModel<ProductFullDto>> GetProductByIdAsync(int productId)
    {
        var entity = await applicationDbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId);

        if (entity is null)
            return ResponseModel<ProductFullDto>.Fail("Product not found", HttpStatusCode.NotFound);

        var productDto = mapper.Map<ProductFullDto>(entity);

        return ResponseModel<ProductFullDto>.Success(productDto, "Product retrieved successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<ProductFullInformationDto>> GetProductFullByIdAsync(int productId)
    {
        var entity = await applicationDbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId);

        if (entity is null)
            return ResponseModel<ProductFullInformationDto>.Fail("Product not found", HttpStatusCode.NotFound);

        var productDto = mapper.Map<ProductFullInformationDto>(entity);

        return ResponseModel<ProductFullInformationDto>.Success(productDto, "Product retrieved successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<ProductFullDto>> UpdateProductAsync(ProductUpdateDto updateDto, int productId)
    {
        var entity = await applicationDbContext.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == productId);

        if (entity is null)
            return ResponseModel<ProductFullDto>.Fail("Product not found", HttpStatusCode.NotFound);

        mapper.Map(updateDto, entity);

        if (updateDto.CategoryId is not null)
        {
            var category = await applicationDbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == updateDto.CategoryId);

            if (category is null)
                return ResponseModel<ProductFullDto>.Fail("Category not found", HttpStatusCode.NotFound);

            entity.CategoryId = updateDto.CategoryId.Value;
            entity.Category = category;
        }

        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<ProductFullDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var productDto = mapper.Map<ProductFullDto>(entity);

        return ResponseModel<ProductFullDto>.Success(productDto, "Product updated successfully", HttpStatusCode.OK);
    }

    public async Task<TableResponse<List<ProductFullDto>>> GetProductByNameAsync(SearchOptions searchOptions)
    {
        var entities = applicationDbContext.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchOptions.SearchTerm))
        {
            entities = entities.Where(x =>
                x.NameUz.StartsWith(searchOptions.SearchTerm) ||
                x.NameEn.StartsWith(searchOptions.SearchTerm));
        }

        var count = await entities.CountAsync();

        var products= await entities
            .Skip(searchOptions.First)
            .Take(searchOptions.Rows)
            .ToListAsync();

        var productDtos = mapper.Map<List<ProductFullDto>>(products);

        return new TableResponse<List<ProductFullDto>>() { Total = count, Items = productDtos };
    }

    public async Task<ResponseModel<ProductFullDto>> AddQuantityAsync(int productId, int quantity)
    {
        if (quantity <= 0)
            return ResponseModel<ProductFullDto>.Fail("Quantity must be greater than 0", HttpStatusCode.BadRequest);

        var product = await applicationDbContext.Products.Include(x=>x.Category).FirstOrDefaultAsync(x => x.Id == productId);

        if (product is null)
            return ResponseModel<ProductFullDto>.Fail("Product not found", HttpStatusCode.NotFound);

        product.StockQuantity += quantity;

        var result = await applicationDbContext.SaveChangesAsync();

        if(result<1)
            return ResponseModel<ProductFullDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var productDto= mapper.Map<ProductFullDto>(product);

        return ResponseModel<ProductFullDto>.Success(productDto, "Stock Quantity updated successfully", HttpStatusCode.OK);
    }
}
