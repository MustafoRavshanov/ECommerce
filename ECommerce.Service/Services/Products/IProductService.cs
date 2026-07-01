using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;

namespace ECommerce.Service.Services.Products
{
    public interface IProductService
    {
        Task<ResponseModel<ProductFullDto>> CreateProductAsync(ProductCreateDto createDto);
        Task<TableResponse<List<ProductFullDto>>> GetAllProductsAsync(TableOptions tableOptions);
        Task<TableResponse<List<ProductFullInformationDto>>> GetAllProductFullAsync(TableOptions tableOptions);
        Task<ResponseModel<ProductFullDto>> GetProductByIdAsync(int productId);
        Task<ResponseModel<ProductFullInformationDto>>GetProductFullByIdAsync(int productId);
        Task<ResponseModel<ProductFullDto>> UpdateProductAsync(ProductUpdateDto updateDto, int productId);
        Task<ResponseModel<bool>> DeleteProductAsync(int productId);
        Task<TableResponse<List<ProductFullDto>>> GetProductByNameAsync(SearchOptions searchOptions);
        Task<ResponseModel<ProductFullDto>> AddQuantityAsync(int productId, int quantity);
    }
}
