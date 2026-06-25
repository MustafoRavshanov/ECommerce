using ECommerce.API.Filters;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Regions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/region")]
[Authorize]
public class RegionController(IRegionService regionService) : ControllerBase
{
    [HttpPost("create")]
    [HasPermission(Permission.AddressesManage)]
    public async Task<ResponseModel<RegionDto>> CreateAsync(RegionCreateDto dto) =>
        await regionService.AddRegionAsync(dto);

    [HttpGet("get-all")]
    public async Task<TableResponse<List<RegionDto>>> GetAllAsync([FromQuery] TableOptions options) =>
        await regionService.GetAllRegionsAsync(options);

    [HttpGet("get-by-id/{id}")]
    public async Task<ResponseModel<RegionDto>> GetByIdAsync([FromRoute] int id) =>
        await regionService.GetRegionByIdAsync(id);

    [HttpPut("update/{id}")]
    [HasPermission(Permission.AddressesManage)]
    public async Task<ResponseModel<RegionDto>> UpdateAsync(RegionUpdateDto dto, [FromRoute] int id) =>
        await regionService.UpdateRegionAsync(dto, id);

    [HttpDelete("delete/{id}")]
    [HasPermission(Permission.AddressesManage)]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int id) =>
        await regionService.DeleteRegionAsync(id);
}
