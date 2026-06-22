using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Districts;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/district")]
public class DistrictController(IDistrictService districtService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<ResponseModel<DistrictDto>> CreateAsync(DistrictCreateDto dto) =>
        await districtService.AddDistrictAsync(dto);

    [HttpGet("get-all")]
    public async Task<TableResponse<List<DistrictDto>>> GetAllAsync([FromQuery] TableOptions options) =>
        await districtService.GetAllDistrictsAsync(options);

    [HttpGet("get-by-id/{id}")]
    public async Task<ResponseModel<DistrictDto>> GetByIdAsync([FromRoute] int id) =>
        await districtService.GetDistrictByIdAsync(id);

    [HttpPut("update/{id}")]
    public async Task<ResponseModel<DistrictDto>> UpdateAsync(DistrictUpdateDto dto, [FromRoute] int id) =>
        await districtService.UpdateDistrictAsync(dto, id);

    [HttpDelete("delete/{id}")]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int id) =>
        await districtService.DeleteDistrictAsync(id);
}
