using ECommerce.Domain.Enums;

namespace ECommerce.Domain.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentType PaymentType { get; set; }
    public DateTime CreatedAt { get; set; }
}
public class OrderFullInformationDto
{
    public int Id { get; set; }
    public string? address { get; set; }
    public string? DistrictName { get; set; }
    public string? RegionName { get; set; }
    public string? CustomerFirstName { get; set; }
    public string? CustomerLastName { get; set; }
    public string? CustomerPhoneNumber { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentType PaymentType { get; set; }
    public string? CancelledReason { get; set; }
    public string? Feedback { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<OrderDetailDto>? Items { get; set; }
}
public class OrderCreateDto
{
    public PaymentType PaymentType { get; set; }
}
public class OrderUpdateDto
{
    public OrderStatus Status { get; set; }
    public string? CancelledReason { get; set; }
    public string? Feedback { get; set; }
}
