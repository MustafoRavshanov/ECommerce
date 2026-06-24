using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;

namespace ECommerce.Service.Services.Customers;

public interface ICustomerService
{
    Task<ResponseModel<CustomerDto>> AddCustomerAsync(CustomerCreateDto createDto);
    Task<ResponseModel<bool>> DeleteCustomerAsync(int id);
    Task<TableResponse<List<CustomerDto>>> GetAllCustomersAsync(TableOptions options);
    Task<TableResponse<List<CustomerFullInformationDto>>> GetAllCustomersFullAsync(TableOptions options);
    Task<ResponseModel<CustomerDto>> GetCustomerByIdAsync(int customerId);
    Task<ResponseModel<CustomerFullInformationDto>> GetCustomerFullByIdAsync(int customerId);
    Task<ResponseModel<CustomerDto>> UpdateCustomerAsync(CustomerUpdateDto updateDto, int customerId);
}
