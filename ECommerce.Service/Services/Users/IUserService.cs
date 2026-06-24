using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;

namespace ECommerce.Service.Services.Users;

public interface IUserService
{
    Task<ResponseModel<UserDto>> CreateUserAsync(UserCreateDto userCreateDto);
    Task<ResponseModel<UserDto>> GetUserByIdAsync(int userId);
    Task<ResponseModel<UserDto>> GetUserByPhoneNumberAsync(string phoneNumber);
    Task<TableResponse<List<UserFullDto>>> GetAllUsersFullAsync(TableOptions options);
    Task<ResponseModel<UserFullDto>> GetUserFullByIdAsync(int userId);
    Task<ResponseModel<UserFullDto>> GetUserFullByPhoneNumberAsync(string phoneNumber);
    Task<ResponseModel<UserDto>> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto);
    Task<ResponseModel<bool>> UpdateUserPasswordAsync(int userId, UserUpdatePasswordDto userUpdatePasswordDto);
}
