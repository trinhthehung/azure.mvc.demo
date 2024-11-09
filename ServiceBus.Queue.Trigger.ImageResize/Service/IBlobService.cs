using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace ServiceBus.Queue.Trigger.ImageResize.ServiceBus;

public interface IBlobService
{ 
    Task<BlobContainerClient> GetBlobContainerClient(string blobContainerName);
    Task<BlobClient> GetBlobAsync(string blobContainerName, string blobName);
}