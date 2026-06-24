using ECommerce.Domain.DTOs;
using ECommerce.Domain.Helper;

namespace ECommerce.Service.Services.Authorizations;

public interface IAuthService
{
    Task<ResponseModel<bool>> SendOtpAsync(SendOtpDto dto);
    Task<ResponseModel<bool>> VerifyOtpAsync(VerifyOtpDto dto);
    Task<ResponseModel<AuthResponseDto>> RegisterAsync(RegisterDto dto);
    Task<ResponseModel<AuthResponseDto>> LoginAsync(LoginDto dto);
}
