using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;
[ApiController]
[Route("api/product")]
public class ProductController(IProductService productService): ControllerBase
{
    [HttpPost("create")]
    public async Task<ResponseModel<ProductDto>> CreateAsync(ProductCreateDto dto) =>
        await productService.AddProductAsync(dto);

    [HttpGet("get-all")]
    public async Task<TableResponse<List<ProductDto>>> GetAllAsync([FromQuery] TableOptions options) =>
        await productService.GetAllProductsAsync(options);

    [HttpGet("get-by-id/{id}")]
    public async Task<ResponseModel<ProductDto>> GetByIdAsync([FromRoute] int id) =>
        await productService.GetProductByIdAsync(id);

    [HttpPut("update/{id}")]
    public async Task<ResponseModel<ProductDto>> UpdateAsync(ProductUpdateDto dto, [FromRoute] int id) =>
        await productService.UpdateProductAsync(dto, id);

    [HttpDelete("delete/{id}")]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int id) =>
        await productService.DeleteProductAsync(id);

}
