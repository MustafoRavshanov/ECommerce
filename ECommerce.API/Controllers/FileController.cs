using ECommerce.API.Filters;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.MinIO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECommerce.API.Controllers
{
    [Route("api/file")]
    [Authorize]
    public class FileController(IFileService fileService):BaseController
    {
        [HasPermission(Permission.ProductsCreate)]
        [HttpPost("upload")]
        public async Task<ResponseModel<string>> UploadFile(IFormFile file)
        {
            if (file is null || file.Length == 0)
                return ResponseModel<string>.Fail("No file uploaded", HttpStatusCode.NotFound);

            using var stream = file.OpenReadStream();

            return await fileService.UploadFileAsync(stream, file.ContentType);
        }

        [HasPermission(Permission.ProductsCreate)]
        [HttpGet("get-by-id/{id}")]
        public async Task<ResponseModel<string>> GetFileUrlByIdAsync([FromRoute] int id) => 
            await fileService.GetUrlByIdAsync(id);

        [HasPermission(Permission.ProductsCreate)]
        [HttpGet("get-all")]
        public async Task<TableResponse<List<string>>> GetAllFileUrlAsync([FromQuery] TableOptions options) => 
            await fileService.GetAllUrlAsync(options);
    }
}
