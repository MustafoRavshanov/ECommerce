using ECommerce.Domain.Helper;

namespace ECommerce.Service.Services.MinIO;

public interface IFileService
{
    Task<ResponseModel<string>> UploadFileAsync(Stream fileStream, string contentType, string fileSign);
    Task<ResponseModel<string>> GetFileUrlByName(string fileSign);
    Task<ResponseModel<List<string>>> FilterUrlsByName(string name);
}
