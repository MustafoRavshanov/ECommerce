using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerce.Service.Services.Districts;

public class DistrictService(ApplicationDbContext applicationDbContext, IMapper mapper) : IDistrictService
{
    public async Task<ResponseModel<DistrictDto>> AddDistrictAsync(DistrictCreateDto createDto)
    {
        var entity = mapper.Map<Domain.Entities.District>(createDto);
        await applicationDbContext.Districts.AddAsync(entity);
        var result = await applicationDbContext.SaveChangesAsync();

        if(result < 1)
            return ResponseModel<DistrictDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var district =await applicationDbContext.Districts.Include(x => x.Region).FirstOrDefaultAsync(x => x.Id == entity.Id);
        var mappedResult = mapper.Map<DistrictDto>(district);

        return ResponseModel<DistrictDto>.Success(mappedResult, "District added successfully", HttpStatusCode.Created);
    }

    public async Task<TableResponse<List<DistrictDto>>> GetAllDistrictsAsync(TableOptions tableOptions)
    {
        var entities = applicationDbContext.Districts.Include(x=>x.Region).AsQueryable();
        var count = await entities.CountAsync();

        var items = await entities
            .Skip(tableOptions.First)
            .Take(tableOptions.Rows)
            .ToListAsync();

        var resultDtos = mapper.Map<List<DistrictDto>>(items);

        return new TableResponse<List<DistrictDto>> { Total = count, Items = resultDtos };
    }

    public async Task<ResponseModel<DistrictDto>> GetDistrictByIdAsync(int districtId)
    {
        var entity = await applicationDbContext.Districts.Include(x=>x.Region).FirstOrDefaultAsync(x => x.Id == districtId);

        if (entity is null)
            return ResponseModel<DistrictDto>.Fail("District not found", HttpStatusCode.NotFound);

        var resultDto = mapper.Map<DistrictDto>(entity);

        return ResponseModel<DistrictDto>.Success(resultDto, "District retrieved successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<bool>> DeleteDistrictAsync(int districtId)
    {
        var entity = await applicationDbContext.Districts.FirstOrDefaultAsync(x => x.Id == districtId);
        if (entity == null)
            return ResponseModel<bool>.Fail("District not found", HttpStatusCode.NotFound);

        applicationDbContext.Remove(entity);
        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<bool>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        return ResponseModel<bool>.Success(true, "District deleted successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<DistrictDto>> UpdateDistrictAsync(DistrictUpdateDto updateDto, int districtId)
    {
        var entity = await applicationDbContext.Districts.Include(x=>x.Region).FirstOrDefaultAsync(x => x.Id == districtId);

        if (entity is null)
            return ResponseModel<DistrictDto>.Fail("District not found", HttpStatusCode.NotFound);

        mapper.Map(updateDto, entity);
        applicationDbContext.Districts.Update(entity);
        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<DistrictDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var resultDto = mapper.Map<DistrictDto>(entity);

        return ResponseModel<DistrictDto>.Success(resultDto, "District updated successfully", HttpStatusCode.OK);
    }
}
