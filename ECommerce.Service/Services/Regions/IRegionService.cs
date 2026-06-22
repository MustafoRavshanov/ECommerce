using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;

namespace ECommerce.Service.Services.Regions;

public interface IRegionService
{
    Task<ResponseModel<RegionDto>> AddRegionAsync(RegionCreateDto regionCreateDto);
    Task<TableResponse<List<RegionDto>>> GetAllRegionsAsync(TableOptions tableOptions);
    Task<ResponseModel<RegionDto>> GetRegionByIdAsync(int regionId);
    Task<ResponseModel<RegionDto>> UpdateRegionAsync(RegionUpdateDto regionUpdateDto, int regionId);
    Task<ResponseModel<bool>> DeleteRegionAsync(int regionId);
}
