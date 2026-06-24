using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Helper;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerce.Service.Services.Customers;

public class CustomerService(ApplicationDbContext applicationDbContext, IMapper mapper) : ICustomerService
{
    public async Task<ResponseModel<CustomerDto>> AddCustomerAsync(CustomerCreateDto createDto)
    {
        var entity=mapper.Map<Customer>(createDto);
        await applicationDbContext.AddAsync(entity);
        var result = await applicationDbContext.SaveChangesAsync();

        if(result<1)
            return ResponseModel<CustomerDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var resultDto= mapper.Map<CustomerDto>(entity);

        return ResponseModel<CustomerDto>.Success(resultDto, "Customer created successfully", HttpStatusCode.Created);
    }

    public async Task<TableResponse<List<CustomerDto>>> GetAllCustomersAsync(TableOptions options)
    {
        var entities = applicationDbContext.Customers.AsQueryable();
        var count = await entities.CountAsync();

        var customers= await entities
            .Skip(options.First)
            .Take(options.Rows)
            .ToListAsync();  

        var resultDtos=mapper.Map<List<CustomerDto>>(customers);

        return new TableResponse<List<CustomerDto>>() { Total=count, Items = resultDtos };
    }

    public async Task<TableResponse<List<CustomerFullInformationDto>>> GetAllCustomersFullAsync(TableOptions options)
    {
        var entities = applicationDbContext.Customers.AsQueryable();
        var count = await entities.CountAsync();

        var customers = await entities
            .Skip(options.First)
            .Take(options.Rows)
            .ToListAsync();

        var resultDtos = mapper.Map<List<CustomerFullInformationDto>>(customers);

        return new TableResponse<List<CustomerFullInformationDto>>() { Total = count, Items = resultDtos };
    }

    public async Task<ResponseModel<CustomerDto>> GetCustomerByIdAsync(int customerId)
    {
        var customer =await applicationDbContext.Customers.FirstOrDefaultAsync(x => x.Id == customerId);

        if (customer is null)
            return ResponseModel<CustomerDto>.Fail("Costumer not found !" , HttpStatusCode.NotFound);

        var resultDto = mapper.Map<CustomerDto>(customer);

        return ResponseModel<CustomerDto>.Success(resultDto, "Customer retrieved successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<CustomerFullInformationDto>> GetCustomerFullByIdAsync(int customerId)
    {
        var customer = await applicationDbContext.Customers.FirstOrDefaultAsync(x => x.Id == customerId);

        if (customer is null)
            return ResponseModel<CustomerFullInformationDto>.Fail("Costumer not found !", HttpStatusCode.NotFound);

        var resultDto = mapper.Map<CustomerFullInformationDto>(customer);

        return ResponseModel<CustomerFullInformationDto>.Success(resultDto, "Customer retrieved successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<CustomerDto>> UpdateCustomerAsync(CustomerUpdateDto updateDto, int customerId)
    {
        var entity= await applicationDbContext.Customers
            .FirstOrDefaultAsync(x=>x.Id == customerId);

        if(entity is null)
             return ResponseModel<CustomerDto>.Fail("Costumer not found", HttpStatusCode.NotFound); 

        mapper.Map(updateDto, entity);
        var result = await applicationDbContext.SaveChangesAsync();

        if(result<1)
            return ResponseModel<CustomerDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var dto=mapper.Map<CustomerDto>(entity);

        return ResponseModel<CustomerDto>.Success(dto, "Customer updated successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<bool>> DeleteCustomerAsync(int id)
    {
        var entity = await applicationDbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            return ResponseModel<bool>.Fail("Costumer not found", HttpStatusCode.NotFound);

        applicationDbContext.Customers.Remove(entity);
        var result = await applicationDbContext.SaveChangesAsync();

        if(result<1)
            return ResponseModel<bool>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        return ResponseModel<bool>.Success(true, "Customer deleted successfully", HttpStatusCode.OK);
    }
}
