
namespace ECommerce.Domain.DTOs;

public class RegionDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class RegionCreateDto
{
    public string? Name { get; set; }
}

public class RegionUpdateDto
{
    public string? Name { get; set; }
}