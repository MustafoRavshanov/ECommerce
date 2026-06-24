
namespace ECommerce.Domain.DTOs;

public class CustomerDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public long ChatId { get; set; }
}

public class CustomerFullInformationDto
{
    public string? DistrictName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public long ChatId { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }
}
public class CustomerCreateDto
{
    public string? Password { get; set; }
    public int RoleId { get; set; }
    public int DistrictId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber{ get; set; }
    public long ChatId { get; set; }
    public string? Address { get; set; }
}
public class CustomerUpdateDto
{
    public int? DistrictId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public long? ChatId { get; set; }
    public string? Address { get; set; }
}
