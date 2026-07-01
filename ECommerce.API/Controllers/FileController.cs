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
        [HttpPost("upload/{fileSign}")]
        public async Task<ResponseModel<string>> UploadFile(IFormFile file, [FromRoute] string fileSign)
        {
            if (file is null || file.Length == 0)
                return ResponseModel<string>.Fail("No file uploaded", HttpStatusCode.NotFound);

            using var stream = file.OpenReadStream();

            return await fileService.UploadFileAsync(stream, file.ContentType, fileSign);
        }

        [HasPermission(Permission.ProductsCreate)]
        [HttpGet("get-by-name/{fileSign}")]
        public async Task<ResponseModel<string>> GetFileUrlByName([FromRoute] string fileSign) => 
            await fileService.GetFileUrlByName(fileSign);

        [HasPermission(Permission.ProductsCreate)]
        [HttpGet("filter-by-name/{fileSign}")]
        public async Task<ResponseModel<List<string>>> FilterUrlsByName([FromRoute] string fileSign) => 
            await fileService.FilterUrlsByName(fileSign);
    }
}
