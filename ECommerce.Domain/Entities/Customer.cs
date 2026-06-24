
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

[Table("customer")]
public class Customer
{
    [Column("id")]
    public int Id { get; set; }

    [Column("district_id")]
    public int? DistrictId { get; set; }

    [Column("first_name")]
    public string? FirstName { get; set; }

    [Column("last_name")]
    public string? LastName { get; set; }

    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    [Column("chat_id")]
    public long ChatId { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    // Navigation property
    public User? User { get; set; }
    public District? District { get; set; }
    public ICollection<Basket>? Baskets { get; set; }
    public ICollection<Order>? Orders { get; set; }
}
