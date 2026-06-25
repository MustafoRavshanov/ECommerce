using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Helper;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerce.Service.Services.Regions;

public class RegionService(ApplicationDbContext applicationDbContext, IMapper mapper) : IRegionService
{
    public async Task<ResponseModel<RegionDto>> AddRegionAsync(RegionCreateDto regionCreateDto)
    {
        var entity= mapper.Map<Region>(regionCreateDto);
        await applicationDbContext.Regions.AddAsync(entity);
        var result= await applicationDbContext.SaveChangesAsync();

        if(result<1)
            return ResponseModel<RegionDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var regionDto= mapper.Map<RegionDto>(entity);

        return ResponseModel<RegionDto>.Success(regionDto, "Region created successfully", HttpStatusCode.Created);
    }

    public async Task<TableResponse<List<RegionDto>>> GetAllRegionsAsync(TableOptions tableOptions)
    {
        var entities = applicationDbContext.Regions.AsQueryable();
        var count = await entities.CountAsync();

        var regions = await entities
            .Skip(tableOptions.First)
            .Take(tableOptions.Rows)
            .ToListAsync();

        var resultDtos = mapper.Map<List<RegionDto>>(regions);

        return new TableResponse<List<RegionDto>>() { Total = count, Items = resultDtos };
    }

    public async Task<ResponseModel<RegionDto>> GetRegionByIdAsync(int regionId)
    {
        var entity = await applicationDbContext.Regions.FirstOrDefaultAsync(x=> x.Id==regionId);

        if( entity is null)
            return ResponseModel<RegionDto>.Fail("Region not found", HttpStatusCode.NotFound);

        var regionDto = mapper.Map<RegionDto>(entity);

        return ResponseModel<RegionDto>.Success(regionDto, "Region retrieved successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<RegionDto>> UpdateRegionAsync(RegionUpdateDto regionUpdateDto, int regionId)
    {
        var entity = await applicationDbContext.Regions.FirstOrDefaultAsync(x => x.Id == regionId);

        if(entity is null)
            return ResponseModel<RegionDto>.Fail("Region not found", HttpStatusCode.NotFound);

        mapper.Map(regionUpdateDto, entity);
        applicationDbContext.Regions.Update(entity);
        var result = await applicationDbContext.SaveChangesAsync();

        if(result<1)
            return ResponseModel<RegionDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var regionDto = mapper.Map<RegionDto>(entity);
        return ResponseModel<RegionDto>.Success(regionDto, "Region updated successfully", HttpStatusCode.OK);
    }
    public async Task<ResponseModel<bool>> DeleteRegionAsync(int regionId)
    {
        var entity= await applicationDbContext.Regions.FirstOrDefaultAsync(x=>x.Id==regionId);

        if(entity is null)
            return ResponseModel<bool>.Fail("Region not found", HttpStatusCode.NotFound);

        applicationDbContext.Regions.Remove(entity);
        var result= await applicationDbContext.SaveChangesAsync();

        if(result<1)
            return ResponseModel<bool>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        return ResponseModel<bool>.Success(true, "Region deleted successfully", HttpStatusCode.OK);
    }
}
