using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Authorizations;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/authorization")]
public class AuthorizationController(IAuthService authService): ControllerBase
{
    [HttpPost("send-otp")]
    public async Task<ResponseModel<bool>> SendOtpAsync([FromBody] SendOtpDto dto) =>
        await authService.SendOtpAsync(dto);

    [HttpPost("verify-otp")]
    public async Task<ResponseModel<bool>> VerifyOtpAsync([FromBody] VerifyOtpDto dto) =>
        await authService.VerifyOtpAsync(dto);

    [HttpPost("register")]
    public async Task<ResponseModel<AuthResponseDto>> RegisterAsync([FromBody] RegisterDto dto) =>
        await authService.RegisterAsync(dto);

    [HttpPost("login")]
    public async Task<ResponseModel<AuthResponseDto>> LoginAsync([FromBody] LoginDto dto) =>
        await authService.LoginAsync(dto);
}
