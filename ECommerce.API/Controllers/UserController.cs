using ECommerce.API.Extentions;
using ECommerce.API.Filters;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController(IUserService userService):ControllerBase
{
    [HttpPost("create-user")]
    [HasPermission(Permission.UserManage)]
    public async Task<ResponseModel<UserDto>> CreateAsync([FromBody]UserCreateDto dto) =>
        await userService.CreateUserAsync(dto);

    [HttpGet("get-all-users")]
    [HasPermission(Permission.UserManage)]
    public async Task<TableResponse<List<UserFullDto>>> GetAllAsync([FromQuery] TableOptions options) =>
        await userService.GetAllUsersFullAsync(options);

    [HttpGet("get-user-by-id/{id}")]
    [HasPermission(Permission.UserManage)]
    public async Task<ResponseModel<UserDto>> GetUserById([FromRoute] int id)=>
        await userService.GetUserByIdAsync(id);

    [HttpGet("get-user-full-by-id/{id}")]
    [HasPermission(Permission.UserManage)]
    public async Task<ResponseModel<UserFullDto>> GetUserFullByIdAsync([FromRoute] int id) =>
        await userService.GetUserFullByIdAsync(id);

    [HttpGet("get-user-by-phone/{phoneNumber}")]
    [HasPermission(Permission.UserManage)]
    public async Task<ResponseModel<UserDto>> GetUserByPhoneNumber([FromRoute] string phoneNumber) =>
        await userService.GetUserByPhoneNumberAsync(phoneNumber);

    [HttpGet("get-user-full-by-phone/{phoneNumber}")]
    [HasPermission(Permission.UserManage)]
    public async Task<ResponseModel<UserFullDto>> GetUserFullByPhoneNumber([FromRoute] string phoneNumber) =>
        await userService.GetUserFullByPhoneNumberAsync(phoneNumber);

    [HttpPut("update-user")]
    public async Task<ResponseModel<UserDto>> UpdateUserAsync( [FromBody] UserUpdateDto updateDto)
    {
        var userId = User.GetUserId();
        return await userService.UpdateUserAsync(userId, updateDto);
    }

    [HttpPut("update-Password")]
    public async Task<ResponseModel<bool>> UpatePasswordAsync( [FromBody] UserUpdatePasswordDto dto)
    {
        var userId = User.GetUserId(); 
        return await userService.UpdateUserPasswordAsync(userId, dto);
    }
}
