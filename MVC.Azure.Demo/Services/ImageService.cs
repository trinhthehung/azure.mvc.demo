using System.Drawing.Imaging;
using MVC.Azure.Demo.Datas;
using Image = System.Drawing.Image;

namespace MVC.Azure.Demo.Services;

public class ImageService:IImageService
{
    private readonly IBlobStorageService _iBlobStorageService;
    private readonly IServiceScopeFactory _iServiceScopeFactory;
    public ImageService(IBlobStorageService iBlobStorageService, IServiceScopeFactory iServiceScopeFactory)
    {
        _iBlobStorageService = iBlobStorageService;
        _iServiceScopeFactory = iServiceScopeFactory;
    }

    public async Task InsertAync(Image image)
    {
    }

    public async Task UpdateAsync()
    {
        
    }

    public async Task DeleteAsync()
    {
        
    }

    private async Task SendEventAsync()
    {
        
    }
    public async Task ImageResizeAsync(string blogName)
    {
        try
        {
            var id = Guid.NewGuid();
            var ImageEx = new ImageEx()
            {
                Id = id,
                ImageId = Guid.Parse(blogName.Split('.')[0]),
                Name = null,
                BlogName = null,
                Extention = null,
                Size = 0,
                ReSize = "",
                Type = null,
                CreateBy = "sys",
                CreateDate = DateTime.Now,
                LastModifyTime = null,
                IsDelete  = false,
            };
            
            var blobClient = await _iBlobStorageService.GetBlobAsync(blogName);
            
            if (blobClient.Exists())
            {
                var file = await blobClient.DownloadContentAsync();
                using (var stream = await blobClient.OpenReadAsync())
                {
                   var fromFile = new FormFile(stream, 0, stream.Length, null,blobClient.Name)
                   {
                       Headers = new HeaderDictionary(),
                       ContentType = file.Value.Details.ContentType
                   };
                   var blodName = await _iBlobStorageService.UploadBlobResizeAsync(fromFile, Guid.NewGuid().ToString(), null);
                   ImageEx.BlogName = blodName;
                   ImageEx.Extention = blodName.Split('.').Last();
                   ImageEx.Size = fromFile.Length;
                   ImageEx.Name = fromFile.FileName;
                   
                   
                   
                   using (var scope = _iServiceScopeFactory.CreateScope())
                   {
                       var context = scope.ServiceProvider.GetService<AppDbContext>();
                       await context.AddAsync(ImageEx);
                       await context.SaveChangesAsync();
                   }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}