
namespace ECommerce.Domain.Helper;

public class SearchOptions
{
    public int First { get; set; } = 0;
    public int Rows { get; set; } = 10;
    public string? SearchTerm { get; set; }
}
