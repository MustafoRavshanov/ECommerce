using ECommerce.Service.Services.MinIO;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/file")]
    public class FileController(IFileService fileService):BaseController
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest("File is not chosen");

            using var stream = file.OpenReadStream();
            var url = await fileService.UploadFileAsync(stream, file.FileName, file.ContentType);

            return Ok(new { imageUrl = url });
        }
    }
}
