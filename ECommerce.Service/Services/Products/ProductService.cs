using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Helper;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Services.Products
{
    public class ProductService(ApplicationDbContext applicationDbContext, IMapper mapper) : IProductService
    {
        public async Task<ResponseModel<ProductDto>> AddProductAsync(ProductCreateDto createDto)
        {
            var category = await applicationDbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == createDto.CategoryId);

            var entity = mapper.Map<Product>(createDto);
            entity.Category = category;

            await applicationDbContext.Products.AddAsync(entity);
            var result = await applicationDbContext.SaveChangesAsync();

            if(result < 1)
                return ResponseModel<ProductDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

            var productDto = mapper.Map<ProductDto>(entity);

            return ResponseModel<ProductDto>.Success(productDto, "Product added successfully", HttpStatusCode.Created);
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

        public async Task<TableResponse<List<ProductDto>>> GetAllProductsAsync(TableOptions tableOptions)
        {
            var entities = applicationDbContext.Products.Include(p => p.Category).AsQueryable();
            var count = await entities.CountAsync();

            var products = await entities
                .Skip(tableOptions.First)
                .Take(tableOptions.Rows)
                .ToListAsync();

            var productDtos = mapper.Map<List<ProductDto>>(products);
           
            return new TableResponse<List<ProductDto>> { Total = count, Items = productDtos };
        }

        public async Task<ResponseModel<ProductDto>> GetProductByIdAsync(int productId)
        {
            var entity = await applicationDbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId);

            if (entity is null)
                return ResponseModel<ProductDto>.Fail("Product not found", HttpStatusCode.NotFound);

            var productDto = mapper.Map<ProductDto>(entity);

            return ResponseModel<ProductDto>.Success(productDto, "Product retrieved successfully", HttpStatusCode.OK);
        }

        public async Task<ResponseModel<ProductDto>> UpdateProductAsync(ProductUpdateDto updateDto, int productId)
        {
            var entity = await applicationDbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId);

            if (entity is null)
                return ResponseModel<ProductDto>.Fail("Product not found", HttpStatusCode.NotFound);

            mapper.Map(updateDto, entity);

            var category = await applicationDbContext.Categories.FirstOrDefaultAsync(c => c.Id == updateDto.CategoryId);
            entity.Category = category;

            var result = await applicationDbContext.SaveChangesAsync();

            if (result < 1)
                return ResponseModel<ProductDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

            var productDto = mapper.Map<ProductDto>(entity);

            return ResponseModel<ProductDto>.Success(productDto, "Product updated successfully", HttpStatusCode.OK);
        }
    }
}
