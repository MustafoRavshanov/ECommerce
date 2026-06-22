using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Services.Products
{
    public interface IProductService
    {
        Task<ResponseModel<ProductDto>> AddProductAsync(ProductCreateDto createDto);
        Task<TableResponse<List<ProductDto>>> GetAllProductsAsync(TableOptions tableOptions);
        Task<ResponseModel<ProductDto>> GetProductByIdAsync(int productId);
        Task<ResponseModel<ProductDto>> UpdateProductAsync(ProductUpdateDto updateDto, int productId);
        Task<ResponseModel<bool>> DeleteProductAsync(int productId);
    }
}
