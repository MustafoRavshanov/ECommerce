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
        Task<ResponseModel<ProductFullDto>> AddProductAsync(ProductCreateDto createDto);
        Task<TableResponse<List<ProductFullDto>>> GetAllProductsAsync(TableOptions tableOptions);
        Task<TableResponse<List<ProductFullInformationDto>>> GetAllProductFullAsync(TableOptions tableOptions);
        Task<ResponseModel<ProductFullDto>> GetProductByIdAsync(int productId);
        Task<ResponseModel<ProductFullInformationDto>>GetProductFullByIdAsync(int productId);
        Task<ResponseModel<ProductFullDto>> UpdateProductAsync(ProductUpdateDto updateDto, int productId);
        Task<ResponseModel<bool>> DeleteProductAsync(int productId);
    }
}
