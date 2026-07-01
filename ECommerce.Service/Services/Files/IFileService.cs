using ECommerce.Domain.Helper;

namespace ECommerce.Service.Services.MinIO;

public interface IFileService
{
    Task<ResponseModel<string>> UploadFileAsync(Stream fileStream, string contentType);
    Task<ResponseModel<string>> GetUrlByIdAsync(int id);
    Task<TableResponse<List<string>>> GetAllUrlAsync(TableOptions options);
}
