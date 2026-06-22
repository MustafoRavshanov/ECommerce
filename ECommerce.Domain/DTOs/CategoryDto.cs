
namespace ECommerce.Domain.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string? NameUz { get; set; } 
    public string? NameEn { get; set; }
}

public class CategoryFullInformationDto 
{
    public int Id { get; set; }
    public string? NameUz { get; set; }
    public string? NameEn { get; set; }
    public List<ProductDto>? Products { get; set; }
}

public class CategoryCreateDto
{
    public string? NameUz { get; set; }
    public string? NameEn { get; set; }
}

public class CategoryUpdateDto
{
    public string? NameUz { get; set; }
    public string? NameEn { get; set; }
}
