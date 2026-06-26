using ECommerce.API.Filters;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/category")]
[Authorize]
public class CategoryController(ICategoryService categoryService) : BaseController
{
    [HttpPost("create")]
    [HasPermission(Permission.CategoriesManage)]
    public async Task<ResponseModel<CategoryDto>> CreateAsync(CategoryCreateDto dto) =>
        await categoryService.AddCategoryAsync(dto);

    [HttpGet("get-all")]
    public async Task<TableResponse<List<CategoryDto>>> GetAllAsync([FromQuery] TableOptions options) =>
        await categoryService.GetAllCategoriesAsync(options);

    [HttpGet("get-all-full")]
    public async Task<TableResponse<List<CategoryFullInformationDto>>> GetAllFullAsync([FromQuery] TableOptions options) =>
        await categoryService.GetAllCategoriesFullAsync(options);

    [HttpGet("get-by-id/{id}")]
    public async Task<ResponseModel<CategoryDto>> GetByIdAsync([FromRoute] int id) =>
        await categoryService.GetCategoryByIdAsync(id);

    [HttpGet("get-full-by-id/{id}")]
    public async Task<ResponseModel<CategoryFullInformationDto>> GetFullByIdAsync([FromRoute] int id) =>
        await categoryService.GetCategoryFullByIdAsync(id);

    [HttpPut("update/{id}")]
    [HasPermission(Permission.CategoriesManage)]
    public async Task<ResponseModel<CategoryDto>> UpdateAsync(CategoryUpdateDto dto, [FromRoute] int id) =>
        await categoryService.UpdateCategoryAsync(dto, id);

    [HttpDelete("delete/{id}")]
    [HasPermission(Permission.CategoriesManage)]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int id) =>
        await categoryService.DeleteCategoryAsync(id);

    [HttpGet("get-by-name")]
    public async Task<TableResponse<List<CategoryDto>>> GetCategoryByName([FromQuery]SearchOptions options) =>
        await categoryService.GetCategoryByNameAsync(options);
}
