using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MVC.Azure.Functions.Queue.Image;

public class QueueTriggerFunctionImage
{
    private readonly ILogger<QueueTriggerFunctionImage> _logger;

    public QueueTriggerFunctionImage(ILogger<QueueTriggerFunctionImage> logger)
    {
        _logger = logger;
    }
    
    [Function(nameof(QueueTriggerFunctionImage))]
    public void Run([QueueTrigger("image_resize"
        ,Connection = "")] 
        QueueMessage message)
    {
        _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
    }
}