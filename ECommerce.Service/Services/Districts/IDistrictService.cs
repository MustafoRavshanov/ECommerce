using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;

namespace ECommerce.Service.Services.Districts;

public interface IDistrictService
{
    Task<ResponseModel<DistrictDto>> AddDistrictAsync(DistrictCreateDto createDto);
    Task<TableResponse<List<DistrictDto>>> GetAllDistrictsAsync(TableOptions tableOptions);
    Task<ResponseModel<DistrictDto>> GetDistrictByIdAsync(int districtId);
    Task<ResponseModel<DistrictDto>> UpdateDistrictAsync(DistrictUpdateDto updateDto, int districtId);
    Task<ResponseModel<bool>> DeleteDistrictAsync(int districtId);
}
