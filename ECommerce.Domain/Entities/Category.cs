
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities;

[Table("category")]
public class Category
{
    [Column("id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("name_uz")]
    public string? NameUz { get; set; }


    [Column("name_en")]
    public string? NameEn { get; set; }
    public required ICollection<Product> Products { get; set; }
}
