using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerce.Service.Services.Categories
{
    public class CategoryService(ApplicationDbContext applicationDbContext, IMapper mapper) : ICategoryService
    {
        public async Task<ResponseModel<CategoryDto>> AddCategoryAsync(CategoryCreateDto createDto)
        {
            var entity = mapper.Map<Domain.Entities.Category>(createDto);
            await applicationDbContext.Categories.AddAsync(entity);
            var result = await applicationDbContext.SaveChangesAsync();

            if (result < 1)
                return ResponseModel<CategoryDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

            var categoryDto = mapper.Map<CategoryDto>(entity);

            return ResponseModel<CategoryDto>.Success(categoryDto, "Category created successfully", HttpStatusCode.Created);
        }

        public async Task<TableResponse<List<CategoryDto>>> GetAllCategoriesAsync(TableOptions tableOptions)
        {
            var entities = applicationDbContext.Categories.AsQueryable();
            var count = await entities.CountAsync();

            var categories = await entities
                .Skip(tableOptions.First)
                .Take(tableOptions.Rows)
                .ToListAsync();

            var categoryDtos = mapper.Map<List<CategoryDto>>(categories);

            return new TableResponse<List<CategoryDto>> { Total = count, Items = categoryDtos };
        }

        public async Task<ResponseModel<CategoryDto>> GetCategoryByIdAsync(int categoryId)
        {
            var entity = await applicationDbContext.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

            if (entity is null)
                return ResponseModel<CategoryDto>.Fail("Category not found", HttpStatusCode.NotFound);

            var categoryDto = mapper.Map<CategoryDto>(entity);

            return ResponseModel<CategoryDto>.Success(categoryDto, "Category retrieved successfully", HttpStatusCode.OK);
        }

        public async Task<ResponseModel<CategoryDto>> UpdateCategoryAsync(CategoryUpdateDto updateDto, int categoryId)
        {
            var entity = await applicationDbContext.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

            if (entity is null)
                return ResponseModel<CategoryDto>.Fail("Category not found", HttpStatusCode.NotFound);

            mapper.Map(updateDto, entity);
            var result = await applicationDbContext.SaveChangesAsync();

            if (result < 1)
                return ResponseModel<CategoryDto>.Fail("Error with updating database", HttpStatusCode.InternalServerError);

            var categoryDto = mapper.Map<CategoryDto>(entity);

            return ResponseModel<CategoryDto>.Success(categoryDto, "Category updated successfully", HttpStatusCode.OK);
        }
        public async Task<ResponseModel<bool>> DeleteCategoryAsync(int categoryId)
        {
            var entity = await applicationDbContext.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

            if (entity is null)
                return ResponseModel<bool>.Fail("Category not found", HttpStatusCode.NotFound);

            applicationDbContext.Categories.Remove(entity);
            var result = await applicationDbContext.SaveChangesAsync();

            if (result < 1)
                return ResponseModel<bool>.Fail("Error with deleting from database", HttpStatusCode.InternalServerError);

            return ResponseModel<bool>.Success(true, "Category deleted successfully", HttpStatusCode.OK);
        }

    }
}
