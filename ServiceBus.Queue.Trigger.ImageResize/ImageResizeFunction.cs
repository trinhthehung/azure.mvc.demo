using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using ServiceBus.Queue.Trigger.ImageResize.Constant;
using ServiceBus.Queue.Trigger.ImageResize.DB;
using ServiceBus.Queue.Trigger.ImageResize.DB.Entity;
using ServiceBus.Queue.Trigger.ImageResize.Dto;
using ServiceBus.Queue.Trigger.ImageResize.ServiceBus;

namespace ServiceBus.Queue.Trigger.ImageResize
{ 

    public class ImageResizeFunction
    {
        private readonly IBlobService _iBlobService;
        private readonly IPublishService _iPublishService;
        private readonly AppDbContext _appDBContext;
        public ImageResizeFunction(IBlobService iBlobService
                                , IPublishService iPublishService
                                , AppDbContext appDbContext)
        {
            _iBlobService = iBlobService;
            _iPublishService = iPublishService;
            _appDBContext = appDbContext;
        }
        
        [FunctionName("ImageResize")]
        public async Task Run(
            [ServiceBusTrigger(ConstantName.QUEUE_IMAGE_RESIZE_PROCESS, Connection = "ServiceBusConnectionString")] string myQueueItem)
        {
            try
            {
                var messageInfo = JsonConvert.DeserializeObject<MessageQueueImageForServiceBusDto>(myQueueItem);
                await this.ResizeImage(messageInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private async Task ResizeImage(MessageQueueImageForServiceBusDto input)
        {
            string blogName = input.BlogName;
            var blobClient = await _iBlobService.GetBlobAsync(ConstantName.BLOB_CONTAINER_IMAGE, blogName);
            var imageResizeId = Guid.NewGuid();
            var ImageEx = new ImageEx()
            {
                Id = imageResizeId,
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
                    var blodName = await UploadBlobResizeAsync(fromFile, imageResizeId.ToString(), null);
                    ImageEx.BlogName = blodName;
                    ImageEx.Extention = blodName.Split('.')?.Last();
                    ImageEx.Size = fromFile.Length;
                    ImageEx.Name = fromFile.FileName;
                }
                var messageImageProcessing = new MessageTopicImageResizeInfoDto()
                {
                    Id = imageResizeId,
                    ImageId = input.ImageId
                };
                await _appDBContext.AddAsync<ImageEx>(ImageEx);
                await _appDBContext.SaveChangesAsync();
                await _iPublishService.SensMessageAsync(ConstantName.TOPIC_IMAGE_RESIZE_PROCESSING,messageImageProcessing);
            }
        }
        public async Task<string> UploadBlobResizeAsync(IFormFile formFile, string imageNameKey, string? originalBlobName = null)
        {
            try
            {
                var blobName = $"{imageNameKey}{Path.GetExtension(formFile.FileName)}";
                using var memoryStream = new MemoryStream();
                formFile.CopyTo(memoryStream);
                memoryStream.Position = 0;
                var blobClient = await _iBlobService.GetBlobAsync(ConstantName.BLOB_CONTAINER_IMAGE_RESIZE, blobName);
                await blobClient.UploadAsync(content: memoryStream, overwrite: true);
                return blobName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}

