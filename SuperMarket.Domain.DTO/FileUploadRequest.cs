using Microsoft.AspNetCore.Http;

namespace SuperMarket.Domain.DTO
{
    public class FileUploadRequest
    {
        public string PathUrl { get; set; }

        public List<IFormFile> Files { get; set; }

        public string FileName { get; set; }

        public bool IsName { get; set; }
    }
}
