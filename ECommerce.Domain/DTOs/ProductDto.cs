
namespace ECommerce.Domain.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string? NameUz { get; set; }
    public string? NameEn { get; set; }
    public decimal Price { get; set; }
    public string? CategoryNameUz { get; set; }
    public int StockQuantity { get; set; }
    public string? ImageUrl { get; set; }
}
public class ProductFullInformationDto
{
    public int Id { get; set; }
    public string? NameUz { get; set; }
    public string? NameEn { get; set; }
    public decimal Price { get; set; }
    public string? CategoryNameUz { get; set; }
    public string? DescriptionUz { get; set; }  
    public string? DescriptionEn { get; set; }
    public int StockQuantity { get; set; }
    public int Weight { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ProductCreateDto
{
    public string? NameUz { get; set; }
    public string? NameEn { get; set; }
    public decimal? Price { get; set; }
    public int? CategoryId { get; set; }
    public string? DescriptionUz { get; set; }
    public string? DescriptionEn { get; set; }
    public int? StockQuantity { get; set; }
    public int? Weight { get; set; }
    public string? ImageUrl { get; set; }
}

public class ProductUpdateDto
{
    public string? NameUz { get; set; }
    public string? NameEn { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public string? DescriptionUz { get; set; }
    public string? DescriptionEn { get; set; }
    public int StockQuantity { get; set; }
    public int Weight { get; set; }
    public string? ImageUrl { get; set; }
}
