// using System.Text;
// using Microsoft.Azure.ServiceBus;
// using MVC.Azure.Demo.Models.Admin;
// using Newtonsoft.Json;
//
// namespace MVC.Azure.Demo.Services;
//
// public class ServiceBusConsumer:IServiceBusConsumer
// {
//     private readonly QueueClient _queueClient;
//     private readonly IConfiguration _configuration;
//     private readonly IImageService _iImageService;
//
//     public ServiceBusConsumer(IConfiguration configuration, IImageService iImageService)
//     {
//         _iImageService = iImageService;
//         _configuration = configuration;
//         _queueClient = new QueueClient(_configuration["Azure:ServiceBus:ConnectionString"], _configuration["Azure:ServiceBus:ImageQueueName"]);
//         
//     }
//
//     public void RegisterOnMessageHandlerAndReceiveMessages()
//     {
//         var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
//         {
//             MaxConcurrentCalls = 1,
//             AutoComplete = false
//         };
//
//         _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
//     }
//
//     private async Task ProcessMessagesAsync(Message message, CancellationToken token)
//     {
//         var myPayload = JsonConvert.DeserializeObject<ImageForServiceBusModel>(Encoding.UTF8.GetString(message.Body));
//         await _iImageService.ImageResizeAsync(myPayload.BlogName);
//         await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
//     }
//
//     private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
//     {
//         _ = exceptionReceivedEventArgs.ExceptionReceivedContext;
//
//         return Task.CompletedTask;
//     }
//
//     public async Task CloseQueueAsync()
//     {
//         await _queueClient.CloseAsync();
//     }
// }