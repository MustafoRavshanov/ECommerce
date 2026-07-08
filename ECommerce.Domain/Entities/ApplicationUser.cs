
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public string? EmailAddress { get; set; }
    public int RoleId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Role? Role { get; set; }
}
