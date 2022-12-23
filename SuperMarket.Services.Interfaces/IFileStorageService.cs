using SuperMarket.Domain.DTO;

namespace SuperMarket.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task<IReadOnlyList<FileUploadReponse>> AddMultipleFileAsync(FileUploadRequest request);
    }
}
