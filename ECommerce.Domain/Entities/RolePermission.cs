using ECommerce.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

[Table("RolePermissions")]
public class RolePermission
{
    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("permission")]
    public Permission Permission { get; set; }

    public Role? Role { get; set; }
}
