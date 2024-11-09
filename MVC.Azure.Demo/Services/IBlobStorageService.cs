using Azure.Storage.Blobs;

namespace MVC.Azure.Demo.Services;

public interface IBlobStorageService
{
    Task<string> GetBlobUrl(string imageNameKey);
    Task RemoveBlob(string imageNameKey);
    Task<string> UploadBlob(IFormFile formFile, string imageNameKey, string? originalBlobName = null);
    Task<BlobClient> GetBlobAsync(string imageNameKey);
    Task<string> UploadBlobResizeAsync(IFormFile formFile, string imageNameKey, string? originalBlobName = null);
}