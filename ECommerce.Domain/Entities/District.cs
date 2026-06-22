
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

[Table("district")]
public class District
{
    [Column("id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("region_id")]
    public int RegionId { get; set; }

    // Navigation property
    public Region? Region { get; set; }

    public ICollection<Customer>? Customers { get; set; }
    public ICollection<Order>? Orders { get; set; }
}
