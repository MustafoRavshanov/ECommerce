namespace ECommerce.Domain.Helper;

public class TableResponse<T>
{
    public int Total { get; set; }
    public T? Items { get; set; }
}
