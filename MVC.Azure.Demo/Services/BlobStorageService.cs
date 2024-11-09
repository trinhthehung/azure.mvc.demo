using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace MVC.Azure.Demo.Services;

public class BlobStorageService: IBlobStorageService
{
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string containerName = "image";
        private readonly string containerNameResize="image-resize";

        public BlobStorageService(IConfiguration configuration, BlobServiceClient blobServiceClient)
        {
            this._configuration = configuration;
            this._blobServiceClient = blobServiceClient;
            containerName = this._configuration["Azure:Storage:ContainerImageBlob"];
        }

        public async Task<string> UploadBlob(IFormFile formFile, string imageNameKey, string? originalBlobName = null)
        {
            try
            {
                var blobName = $"{imageNameKey}{Path.GetExtension(formFile.FileName)}";
                var container = _blobServiceClient.GetBlobContainerClient(containerName); ;

                if (!string.IsNullOrEmpty(originalBlobName))
                {
                    await RemoveBlob(originalBlobName);
                }

                using var memoryStream = new MemoryStream();
                formFile.CopyTo(memoryStream);
                memoryStream.Position = 0;
                var blob = container.GetBlobClient(blobName);
                await blob.UploadAsync(content: memoryStream, overwrite: true);
                return blobName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        
        
        public async Task<string> UploadBlobResizeAsync(IFormFile formFile, string imageNameKey, string? originalBlobName = null)
        {
            try
            {
                var blobName = $"{imageNameKey}{Path.GetExtension(formFile.FileName)}";
                var container = _blobServiceClient.GetBlobContainerClient(containerNameResize); ;

                if (!string.IsNullOrEmpty(originalBlobName))
                {
                    await RemoveBlob(originalBlobName);
                }

                using var memoryStream = new MemoryStream();
                formFile.CopyTo(memoryStream);
                memoryStream.Position = 0;
                var blob = container.GetBlobClient(blobName);
                await blob.UploadAsync(content: memoryStream, overwrite: true);
                return blobName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<string> GetBlobUrl(string imageNameKey)
        {
            try
            {
                var container = await this.GetBlobContainerClient();
                var blob = container.GetBlobClient(imageNameKey);
                BlobSasBuilder blobSasBuilder = new()
                {
                    BlobContainerName = blob.BlobContainerName,
                    BlobName = blob.Name,
                    ExpiresOn = DateTime.UtcNow.AddMinutes(2),
                    Protocol = SasProtocol.Https,
                    Resource = "b",
                };
                blobSasBuilder.SetPermissions(BlobAccountSasPermissions.Read);
                return blob.GenerateSasUri(blobSasBuilder).ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<BlobClient> GetBlobAsync(string imageNameKey)
        {
            try
            {
                var container = await this.GetBlobContainerClient();
                var blobClient = container.GetBlobClient(imageNameKey);
                return blobClient;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task RemoveBlob(string imageNameKey)
        {
            try
            {
                var container = _blobServiceClient.GetBlobContainerClient(containerName);
                var blob = container.GetBlobClient(imageNameKey);
                await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<BlobContainerClient> GetBlobContainerClient()
        {
            try
            {
                BlobContainerClient container = new BlobContainerClient(_configuration["Azure:Storage:ConnectionString"], containerName);
                await container.CreateIfNotExistsAsync();
                return container;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }
    }