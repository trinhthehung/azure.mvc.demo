using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceBus.Queue.Trigger.ImageResize.Constant;
using ServiceBus.Queue.Trigger.ImageResize.DB;
using ServiceBus.Queue.Trigger.ImageResize.Dto;
using ServiceBus.Queue.Trigger.ImageResize.ServiceBus;

namespace ServiceBus.Queue.Trigger.ImageResize.SubService;

public class ImageResizeSucessFunction
{
    private readonly AppDbContext _appDBContext;
    private readonly IPublishService _iPublishService;
    public ImageResizeSucessFunction(AppDbContext appDbContext
                                , IPublishService iPublishService)
    {
          _appDBContext = appDbContext;
          _iPublishService = iPublishService;
    }    
    [FunctionName("ImageResizeSuccessFunction")]
    public void Run([ServiceBusTrigger(ConstantName.TOPIC_IMAGE_RESIZE_PROCESSING,subscriptionName:"azure-demo-subscription"
                                                ,Connection = "ServiceBusConnectionString")]string mySbMsg)
    {
        try
        {
            var infoImage = JsonConvert.DeserializeObject<MessageTopicImageResizeInfoDto>(mySbMsg);
            var  imgae = _appDBContext.Image.Where((x=>x.Id == infoImage.ImageId)).FirstOrDefault();
            if (imgae != null)
            {
                imgae.Status = 2;
                _appDBContext.SaveChanges();
            }
            var lst = _appDBContext.Image.ToList();
            Console.WriteLine($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            Task.Run(() => _iPublishService.SensMessageAsync(ConstantName.QUEUE_IMAGE_RESIZE_COMPLETED, infoImage));
            return;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }
}