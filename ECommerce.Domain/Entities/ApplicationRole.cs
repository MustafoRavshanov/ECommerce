using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {
       public ICollection<RolePermission>? RolePermissions { get; set; }
       public ICollection<ApplicationUser>? Users { get; set; } 
    }
}
