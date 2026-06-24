using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService):ControllerBase
{
    [HttpPost("create-user")]
    public async Task<ResponseModel<UserDto>> CreateAsync([FromBody]UserCreateDto dto) =>
        await userService.CreateUserAsync(dto);

    [HttpGet("get-all-users")]
    public async Task<TableResponse<List<UserFullDto>>> GetAllAsync([FromQuery] TableOptions options) =>
        await userService.GetAllUsersFullAsync(options);

    [HttpGet("get-user-by-id/{id}")]
    public async Task<ResponseModel<UserDto>> GetUserById([FromRoute] int id)=>
        await userService.GetUserByIdAsync(id);

    [HttpGet("get-user-full-by-id/{id}")]
    public async Task<ResponseModel<UserFullDto>> GetUserFullByIdAsync([FromRoute] int id) =>
        await userService.GetUserFullByIdAsync(id);

    [HttpGet("get-user-by-phone/{phoneNumber}")]
    public async Task<ResponseModel<UserDto>> GetUserByPhoneNumber([FromRoute] string phoneNumber) =>
        await userService.GetUserByPhoneNumberAsync(phoneNumber);

    [HttpGet("get-user-full-by-phone/{phoneNumber}")]
    public async Task<ResponseModel<UserFullDto>> GetUserFullByPhoneNumber([FromRoute] string phoneNumber) =>
        await userService.GetUserFullByPhoneNumberAsync(phoneNumber);

    [HttpPut("update-user/{id}")]
    public async Task<ResponseModel<UserDto>> UpdateUserAsync([FromRoute] int id, [FromBody]UserUpdateDto updateDto) =>
        await userService.UpdateUserAsync(id, updateDto);

    [HttpPut("update-Password/{id}")]
    public async Task<ResponseModel<bool>> UpatePasswordAsync([FromRoute] int id, [FromBody] UserUpdatePasswordDto dto) =>
        await userService.UpdateUserPasswordAsync(id, dto);
}
