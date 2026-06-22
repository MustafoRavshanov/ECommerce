using ECommerce.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

[Table("order")]
public class Order
{
    [Column("id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("district_id")]
    public int DistrictId { get; set; }

    [Column("total_price")]
    public decimal TotalPrice { get; set; }

    [Column("status")]
    public OrderStatus Status { get; set; }

    [Column("payment_type")]
    public PaymentType PaymentType { get; set; }

    [Column("cancelled_reason")]
    public string? CancelledReason { get; set; }

    [Column("feedback")]
    public string? Feedback { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    // Navigation property
    public Customer? Customer { get; set; }
    public District? District { get; set; }
    public ICollection<OrderDetail>? OrderDetails { get; set; }
    public string? Address { get; set; }
}
