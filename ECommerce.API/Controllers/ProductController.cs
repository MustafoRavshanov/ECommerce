using ECommerce.API.Filters;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;
[ApiController]
[Route("api/product")]
[Authorize]
public class ProductController(IProductService productService): ControllerBase
{
    [HttpPost("create")]
    [HasPermission(Permission.ProductsManage)]
    public async Task<ResponseModel<ProductFullDto>> CreateAsync(ProductCreateDto dto) =>
        await productService.AddProductAsync(dto);

    [HttpGet("get-all")]
    public async Task<TableResponse<List<ProductFullDto>>> GetAllAsync([FromQuery] TableOptions options) =>
        await productService.GetAllProductsAsync(options);

    [HttpGet("get-all-full")]
    [HasPermission(Permission.ProductsManage)]
    public async Task<TableResponse<List<ProductFullInformationDto>>> GetAllFullAsync([FromQuery] TableOptions options) =>
        await productService.GetAllProductFullAsync(options);

    [HttpGet("get-by-id/{id}")]
    public async Task<ResponseModel<ProductFullDto>> GetByIdAsync([FromRoute] int id) =>
        await productService.GetProductByIdAsync(id);

    [HttpGet("get-full-by-id/{id}")]
    [HasPermission(Permission.ProductsManage)]
    public async Task<ResponseModel<ProductFullInformationDto>> GetFullByIdAsync([FromRoute] int id) =>
    await productService.GetProductFullByIdAsync(id);

    [HttpPut("update/{id}")]
    [HasPermission(Permission.ProductsManage)]
    public async Task<ResponseModel<ProductFullDto>> UpdateAsync(ProductUpdateDto dto, [FromRoute] int id) =>
        await productService.UpdateProductAsync(dto, id);

    [HttpDelete("delete/{id}")]
    [HasPermission(Permission.ProductsManage)]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int id) =>
        await productService.DeleteProductAsync(id);

}
