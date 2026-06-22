

namespace ECommerce.Domain.DTOs;

public class DistrictDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? RegionName { get; set; }
}

public class DistrictCreateDto
{
    public string? Name { get; set; }
    public int RegionId { get; set; }
}

public class DistrictUpdateDto
{
    public string? Name { get; set; }
    public int? RegionId { get; set; }
}
