
namespace ECommerce.Domain.DTOs;

public class OrderDetailDto
{
    public int Id { get; set; }
    public string? ProductNameUz { get; set; }
    public string? ProductNameEn { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
}
public class OrderDetailFullInformationDto
{
    public int Id { get; set; }
    public string? ProductNameUz { get; set; }
    public string? ProductNameEn { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
    public DateTime CreatedAt { get; set; }
}

public class OrderDetailCreateDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
