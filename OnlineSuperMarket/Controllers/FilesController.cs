using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperMarket.Domain.DTO;
using SuperMarket.Services.Interfaces;

namespace OnlineSuperMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly IFileStorageService fileStorageService;

        public FilesController(IFileStorageService fileStorageService)
        {
            this.fileStorageService = fileStorageService;
        }

        [HttpPost]
        public async Task<ApiResponseModel<IReadOnlyList<FileUploadReponse>>> AddFiles([FromForm] FileUploadRequest request)
        {
            var result = await fileStorageService.AddMultipleFileAsync(request);

            return new ApiResponseModel<IReadOnlyList<FileUploadReponse>>(result);
        }
    }
}
