
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

[Table("role")]
public class Role
{
    [Column("id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    public ICollection<User>? Users { get; set; }
    public ICollection<RolePermission>? RolePermissions { get; set; }
}
