using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SuperMarket.Domain.DTO;
using SuperMarket.Services.Interfaces;
using System.Net.Http.Headers;

namespace SuperMarket.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IHostingEnvironment _appEnvironment;
        public FileStorageService(IHostingEnvironment appEnvironmen)
        {
            _appEnvironment = appEnvironmen;
        }

        public async Task<IReadOnlyList<FileUploadReponse>> AddMultipleFileAsync(FileUploadRequest request)
        {
            try
            {
                var result = new List<FileUploadReponse>();

                if (request.Files != null && request.Files.Count > 0)
                {
                    string webRoot = _appEnvironment.WebRootPath;
                    if (string.IsNullOrWhiteSpace(_appEnvironment.WebRootPath))
                    {
                        webRoot = _appEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    }
                    string path = Path.Combine(webRoot, request.PathUrl);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    int count = 0;
                    foreach (IFormFile file in request.Files)
                    {
                        count++;
                        string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        if (!request.IsName)
                        {
                            FileInfo fi = new FileInfo(fileName);
                            string extn = fi.Extension;
                            fileName = String.Concat(count.ToString(), extn);
                        }

                        string fullPath = Path.Combine(path, fileName);

                        if (file.Length > 0)
                        {
                            using (Stream stream = new FileStream(fullPath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        }
                        result.Add(new FileUploadReponse() { Name = fileName, Length = file.Length });
                    }

                    return result;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
