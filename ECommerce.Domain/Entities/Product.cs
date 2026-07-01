
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

[Table("product")]
public class Product
{
    [Column("id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("name_uz")]
    public string? NameUz { get; set; }

    [Column("name_en")]
    public string? NameEn { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("description_uz")]
    public string? DescriptionUz { get; set; }

    [Column("description_en")]
    public string? DescriptionEn { get; set; }

    [Column("stock_quantity")]
    public int StockQuantity { get; set; }

    [Column("weight")]
    public int Weight { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("file_data_id")]
    public int FileDataId { get; set; }


    // Navigation property
    public Category? Category { get; set; }
    public ICollection<Basket>? Baskets { get; set; }
    public ICollection<OrderDetail>? OrderDetails { get; set; }
    public int Stock { get; set; }
}
