using ECommerce.Domain.Enums;

namespace ECommerce.Domain.DTOs;

public class RoleDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<UserDto>? Users { get; set; }
    public List<Permission>? Permissions { get; set; }
}

public class RoleCreateDto
{
    public string? Name { get; set; }
    public List<Permission>? Permissions { get; set; }
}

public class RoleUpdateDto
{
    public string? Name { get; set; }
}