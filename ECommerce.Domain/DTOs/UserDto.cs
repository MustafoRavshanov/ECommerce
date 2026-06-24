using ECommerce.Domain.Enums;
using System.ComponentModel.DataAnnotations;


namespace ECommerce.Domain.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; }
    public string? RoleName { get; set; }
}
public class UserFullDto
{
    public int Id { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; }
    public string? RoleName { get; set; }
    public List<Permission>? Permissions { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UserCreateDto
{
    public string? PhoneNumber { get; set; }

    [MinLength(7, ErrorMessage = "Parol kamida 7 ta belgi bo'lishi kerak")]
    [MaxLength(14, ErrorMessage = "Parol ko'pi bilan 14 ta belgi bo'lishi kerak")]
    public string? Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int RoleId { get; set; }
}

public class UserUpdateDto
{
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

public class UserUpdatePasswordDto
{
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmPassword { get; set; }
}

