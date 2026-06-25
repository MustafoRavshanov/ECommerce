using ECommerce.API.Extentions;
using ECommerce.API.Filters;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/customer")]
[Authorize]
public class CustomerController(ICustomerService customerService): ControllerBase
{
    [HttpPost("create")]
    [HasPermission(Permission.UserManage)]
    public async Task<ResponseModel<CustomerDto>> CreateAsync(CustomerCreateDto dto)=>
        await customerService.AddCustomerAsync(dto);

    [HttpGet("get-all")]
    [HasPermission(Permission.UserManage)]
    public async Task<TableResponse<List<CustomerDto>>> GetAllAsync([FromQuery] TableOptions options)=>
        await customerService.GetAllCustomersAsync(options);

    [HttpGet("get-all-full")]
    [HasPermission(Permission.UserManage)]
    public async Task<TableResponse<List<CustomerFullInformationDto>>> GetAllFullAsync([FromQuery] TableOptions options) =>
        await customerService.GetAllCustomersFullAsync(options);

    [HttpGet("get-full-by-id/{id}")]
    [HasPermission(Permission.UserManage)]
    public async Task<ResponseModel<CustomerFullInformationDto>> GetFullByIdAsync([FromRoute] int id)=>
        await customerService.GetCustomerFullByIdAsync(id);

    [HttpGet("get-by-id/{id}")]
    [HasPermission(Permission.UserManage)]
    public async Task<ResponseModel<CustomerDto>> GetByIdAsync([FromRoute] int id) =>
       await customerService.GetCustomerByIdAsync(id);

    [HttpPut("update")]
    public async Task<ResponseModel<CustomerDto>> UpdateAsync(CustomerUpdateDto dto)
    {
        var customerId = User.GetUserId();
        return await customerService.UpdateCustomerAsync(dto, customerId);
    }

    [HttpDelete("delete/{id}")]
    [HasPermission(Permission.UserManage)]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int id) =>
        await customerService.DeleteCustomerAsync(id);
}
