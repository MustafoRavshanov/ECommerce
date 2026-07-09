
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public override string? UserName { get; set; }
    public bool IsActive { get; set; }
    public int RoleId { get; set; }

    public ApplicationRole? Role { get; set; }
    public Customer? Customer { get; set; }
}
