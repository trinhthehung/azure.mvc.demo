using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace ServiceBus.Queue.Trigger.ImageResize.ServiceBus;

public class BlobService : IBlobService
{
    public BlobService()
    {
        
    }
    public async Task<BlobContainerClient> GetBlobContainerClient(string blobContainerName)
    {
        try
        {
            var BlogStorageConnectionString = Environment.GetEnvironmentVariable("BlogStorageConnectionString");
            var storageConnectionStr = BlogStorageConnectionString;
            BlobContainerClient container = new BlobContainerClient(storageConnectionStr, blobContainerName);
            await container.CreateIfNotExistsAsync();
            return container;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    
    public async Task<BlobClient> GetBlobAsync(string blobContainerName,string blobName)
    {
        try
        {
            var container = await this.GetBlobContainerClient(blobContainerName);
            var blobClient = container.GetBlobClient(blobName);
            return blobClient;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    
    
}