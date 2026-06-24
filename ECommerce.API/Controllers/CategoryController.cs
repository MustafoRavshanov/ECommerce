using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Categories;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;
[ApiController]
[Route("api/category")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpPost("create")]
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
    public async Task<ResponseModel<CategoryDto>> UpdateAsync(CategoryUpdateDto dto, [FromRoute] int id) =>
        await categoryService.UpdateCategoryAsync(dto, id);

    [HttpDelete("delete/{id}")]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int id) =>
        await categoryService.DeleteCategoryAsync(id);
}
