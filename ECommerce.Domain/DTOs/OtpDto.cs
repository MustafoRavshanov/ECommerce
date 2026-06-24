
namespace ECommerce.Domain.DTOs;

public class SendOtpDto
{
    public string? PhoneNumber { get; set; }
}

public class VerifyOtpDto
{
    public string? PhoneNumber { get; set; }
    public required int Code { get; set; }
}

public class RegisterDto
{
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    public string? ConfirmedPassword { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

public class LoginDto
{ 
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
}

public class AuthResponseDto
{ 
    public string? AccessToken { get; set; }
    public string? TokenType { get; set; } = "Bearer";
    public DateTime ExpiresAt { get; set; }
    public UserDto? UserDto { get; set; }
}
