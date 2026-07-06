using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace ECommerce.Service.Services.Categories;

public class CategoryService(ApplicationDbContext applicationDbContext, IMapper mapper, IMemoryCache memoryCache) : ICategoryService
{
    private const string CacheKey = "categories-all";
    private string CacheKeyById(int categoryId) => $"category-{categoryId}";

    public async Task<ResponseModel<CategoryDto>> AddCategoryAsync(CategoryCreateDto createDto)
    {
        var existCategory = await applicationDbContext.Categories
            .FirstOrDefaultAsync(x => x.NameEn == createDto.NameEn || x.NameUz == createDto.NameUz);

        if (existCategory != null)
            return ResponseModel<CategoryDto>.Fail("This category name already exists", HttpStatusCode.Conflict);

        var entity = mapper.Map<Domain.Entities.Category>(createDto);
        await applicationDbContext.Categories.AddAsync(entity);
        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<CategoryDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var categoryDto = mapper.Map<CategoryDto>(entity);
        
        if(memoryCache.TryGetValue(CacheKey, out List<CategoryDto>? cachedCategories))
        {
            cachedCategories!.Add(categoryDto);
            memoryCache.Set(CacheKey, cachedCategories, TimeSpan.FromMinutes(10));
        }
        else
        {
            memoryCache.Set(CacheKeyById(categoryDto.Id), categoryDto, TimeSpan.FromMinutes(10));
        }

        return ResponseModel<CategoryDto>.Success(categoryDto, "Category created successfully", HttpStatusCode.Created);
    }

    public async Task<TableResponse<List<CategoryDto>>> GetAllCategoriesAsync(TableOptions tableOptions)
    {
        if (memoryCache.TryGetValue(CacheKey, out List<CategoryDto>? cachedCategories))
        {
            var cachedCount = cachedCategories!.Count();

            var pagedCategories = cachedCategories!
                .Skip(tableOptions.First)
                .Take(tableOptions.Rows)
                .ToList();

            return new TableResponse<List<CategoryDto>> { Total = cachedCount, Items = pagedCategories };
        }
        
        var entities = applicationDbContext.Categories.AsQueryable();
        var entitiesDto = mapper.Map<List<CategoryDto>>(entities);
        memoryCache.Set(CacheKey, entitiesDto, TimeSpan.FromMinutes(10));
        var count = await entities.CountAsync();

        var categories = await entities
            .Skip(tableOptions.First)
            .Take(tableOptions.Rows)
            .ToListAsync();

        var categoryDtos = mapper.Map<List<CategoryDto>>(categories);

        return new TableResponse<List<CategoryDto>> { Total = count, Items = categoryDtos };
    }

    public async Task<TableResponse<List<CategoryFullInformationDto>>> GetAllCategoriesFullAsync(TableOptions tableOptions)
    {
        if (memoryCache.TryGetValue(CacheKey, out List<CategoryFullInformationDto>? cachedCategories))
        { 
            var cachedCount = cachedCategories!.Count();

            var pagedCategories = cachedCategories!
                .Skip(tableOptions.First)
                .Take(tableOptions.Rows)
                .ToList();

            return new TableResponse<List<CategoryFullInformationDto>> { Total = cachedCount, Items = pagedCategories };
        }

        var entities = applicationDbContext.Categories.Include(c=>c.Products).AsQueryable();
        var entitiesDto = mapper.Map<List<CategoryFullInformationDto>>(entities);
        memoryCache.Set(CacheKey, entitiesDto, TimeSpan.FromMinutes(10));
        var count = await entities.CountAsync();

        var categories = await entities
            .Skip(tableOptions.First)
            .Take(tableOptions.Rows)
            .ToListAsync();

        var categoryDtos = mapper.Map<List<CategoryFullInformationDto>>(categories);

        return new TableResponse<List<CategoryFullInformationDto>> { Total = count, Items = categoryDtos };
    }

    public async Task<ResponseModel<CategoryDto>> GetCategoryByIdAsync(int categoryId)
    {
        var cacheKeyById = CacheKeyById(categoryId);

        if (memoryCache.TryGetValue(cacheKeyById, out CategoryDto? cachedCategory))
            return ResponseModel<CategoryDto>.Success(cachedCategory!, "Category retrieved successfully", HttpStatusCode.OK);

        var entity = await applicationDbContext.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

        if (entity is null)
            return ResponseModel<CategoryDto>.Fail("Category not found", HttpStatusCode.NotFound);

        var categoryDto = mapper.Map<CategoryDto>(entity);
        memoryCache.Set(cacheKeyById, categoryDto, TimeSpan.FromMinutes(10));

        return ResponseModel<CategoryDto>.Success(categoryDto, "Category retrieved successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<CategoryFullInformationDto>> GetCategoryFullByIdAsync(int categoryId)
    {
        var cacheKeyById = CacheKeyById(categoryId);

        if (memoryCache.TryGetValue(cacheKeyById, out CategoryFullInformationDto? cachedCategory))
            return ResponseModel<CategoryFullInformationDto>.Success(cachedCategory!, "Category retrieved successfully", HttpStatusCode.OK);

        var entity = await applicationDbContext.Categories.Include(c=>c.Products).FirstOrDefaultAsync(x => x.Id == categoryId);

        if (entity is null)
            return ResponseModel<CategoryFullInformationDto>.Fail("Category not found", HttpStatusCode.NotFound);

        var categoryDto = mapper.Map<CategoryFullInformationDto>(entity);
        memoryCache.Set(cacheKeyById, categoryDto, TimeSpan.FromMinutes(10));

        return ResponseModel<CategoryFullInformationDto>.Success(categoryDto, "Category retrieved successfully", HttpStatusCode.OK);
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
        memoryCache.Remove(CacheKey);

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

        memoryCache.Remove(CacheKey);

        return ResponseModel<bool>.Success(true, "Category deleted successfully", HttpStatusCode.OK);
    }

    public async Task<TableResponse<List<CategoryDto>>> GetCategoryByNameAsync(SearchOptions searchOptions)
    {
        if (memoryCache.TryGetValue(CacheKey, out List<CategoryDto>? cachedCategories))
        {
            if (!string.IsNullOrWhiteSpace(searchOptions.SearchTerm))
            {
                cachedCategories = cachedCategories!.Where(x =>
                    x.NameUz!.StartsWith(searchOptions.SearchTerm) ||
                    x.NameEn!.StartsWith(searchOptions.SearchTerm)).ToList();
            }

            var cachedCount = cachedCategories!.Count();

            var pagedCategories = cachedCategories!
                .Skip(searchOptions.First)
                .Take(searchOptions.Rows)
                .ToList();

            return new TableResponse<List<CategoryDto>>() { Total = cachedCount, Items = pagedCategories };
        }

        var entities= applicationDbContext.Categories.AsQueryable();
        var entityDtos = mapper.Map<List<CategoryDto>>(entities);
        memoryCache.Set(CacheKey, entityDtos, TimeSpan.FromMinutes(10));

        if (!string.IsNullOrWhiteSpace(searchOptions.SearchTerm))
        {
            entities = entities.Where(x =>
                x.NameUz!.StartsWith(searchOptions.SearchTerm) ||
                x.NameEn!.StartsWith(searchOptions.SearchTerm));
        }

        var count = await entities.CountAsync();

        var categories = await entities
            .Skip(searchOptions.First)
            .Take(searchOptions.Rows)
            .ToListAsync();

        var categoriesDto = mapper.Map<List<CategoryDto>>(categories);
       
        return new TableResponse<List<CategoryDto>>() { Total = count, Items = categoriesDto };
    }
}
