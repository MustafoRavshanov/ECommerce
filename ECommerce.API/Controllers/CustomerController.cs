using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Customers;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/customer")]
public class CustomerController(ICustomerService customerService): ControllerBase
{
    [HttpPost("create")]
    public async Task<ResponseModel<CustomerDto>> CreateAsync(CustomerCreateDto dto)=>
        await customerService.AddCustomerAsync(dto);

    [HttpGet("get-all")]
    public async Task<TableResponse<List<CustomerDto>>> GetAllAsync([FromQuery] TableOptions options)=>
        await customerService.GetAllCustomersAsync(options);

    [HttpGet("get-all-full")]
    public async Task<TableResponse<List<CustomerFullInformationDto>>> GetAllFullAsync([FromQuery] TableOptions options) =>
        await customerService.GetAllCustomersFullAsync(options);

    [HttpGet("get-full-by-id/{id}")]
    public async Task<ResponseModel<CustomerFullInformationDto>> GetFullByIdAsync([FromRoute] int id)=>
        await customerService.GetCustomerFullByIdAsync(id);

    [HttpGet("get-by-id/{id}")]
    public async Task<ResponseModel<CustomerDto>> GetByIdAsync([FromRoute] int id) =>
       await customerService.GetCustomerByIdAsync(id);

    [HttpPut("update/{id}")]
    public async Task<ResponseModel<CustomerDto>> UpdateAsync(CustomerUpdateDto dto, [FromRoute] int id)=>
        await customerService.UpdateCustomerAsync(dto, id);

    [HttpDelete("delete/{id}")]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int id) =>
        await customerService.DeleteCustomerAsync(id);
}
