
namespace ECommerce.Domain.DTOs;

public class BasketDto
{
    public List<BasketItemDto>? Items { get; set; }
    public decimal TotalPrice => Items?.Sum(x => x.TotalPrice)?? 0;
}

public class BasketItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public decimal ProductPrice { get; set; }
    public string? ProductName { get; set; }
    public string? ProductImageUrl { get; set; }

    public decimal TotalPrice => Quantity * ProductPrice;
    public DateTime CreatedAt { get; set; }
}

public class BasketCreateDto 
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
public class BasketUpdateDto
{
    public int? Quantity { get;set; }
}
