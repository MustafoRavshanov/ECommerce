using ECommerce.API.Filters;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Districts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/district")]
[Authorize]
public class DistrictController(IDistrictService districtService) : BaseController
{
    [HttpPost("create")]
    [HasPermission(Permission.AddressesManage)]
    public async Task<ResponseModel<DistrictDto>> CreateAsync(DistrictCreateDto dto) =>
        await districtService.AddDistrictAsync(dto);

    [HttpGet("get-all")]
    [HasPermission(Permission.AddressesManage)]
    public async Task<TableResponse<List<DistrictDto>>> GetAllAsync([FromQuery] TableOptions options) =>
        await districtService.GetAllDistrictsAsync(options);

    [HttpGet("get-by-id/{id}")]
    [HasPermission(Permission.AddressesManage)]
    public async Task<ResponseModel<DistrictDto>> GetByIdAsync([FromRoute] int id) =>
        await districtService.GetDistrictByIdAsync(id);

    [HttpPut("update/{id}")]
    [HasPermission(Permission.AddressesManage)]
    public async Task<ResponseModel<DistrictDto>> UpdateAsync(DistrictUpdateDto dto, [FromRoute] int id) =>
        await districtService.UpdateDistrictAsync(dto, id);

    [HttpDelete("delete/{id}")]
    [HasPermission(Permission.AddressesManage)]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int id) =>
        await districtService.DeleteDistrictAsync(id);
}
