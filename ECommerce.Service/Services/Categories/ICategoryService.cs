using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;

namespace ECommerce.Service.Services.Categories;

public interface ICategoryService
{
    Task<ResponseModel<CategoryDto>> AddCategoryAsync(CategoryCreateDto createDto);
    Task<TableResponse<List<CategoryDto>>> GetAllCategoriesAsync(TableOptions tableOptions);
    Task<TableResponse<List<CategoryFullInformationDto>>> GetAllCategoriesFullAsync(TableOptions tableOptions);
    Task<ResponseModel<CategoryDto>> GetCategoryByIdAsync(int categoryId);
    Task<ResponseModel<CategoryFullInformationDto>> GetCategoryFullByIdAsync(int categoryId);
    Task<ResponseModel<CategoryDto>> UpdateCategoryAsync(CategoryUpdateDto updateDto, int categoryId);
    Task<ResponseModel<bool>> DeleteCategoryAsync(int categoryId);
    Task<TableResponse<List<CategoryDto>>> GetCategoryByNameAsync(SearchOptions searchOptions);
}
