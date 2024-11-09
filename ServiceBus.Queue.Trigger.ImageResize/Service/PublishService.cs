using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
namespace ServiceBus.Queue.Trigger.ImageResize.ServiceBus;

public class PublishService : IPublishService
{
    private readonly ServiceBusClient _serviceBusClient;
    
    public PublishService(ServiceBusClient serviceBusClient)
    {
        _serviceBusClient = serviceBusClient;
    }
    public async Task SensMessageAsync<T>(string topicName, T message) where T: class
    {
        try
        {
            var sender = _serviceBusClient.CreateSender(topicName);
            using ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync();
            var mess = JsonConvert.SerializeObject(message);
            if (!batch.TryAddMessage(new ServiceBusMessage(mess)))
            {
                Console.WriteLine($"Message was not added to the batch");
            }
            await sender.SendMessagesAsync(batch);
            Console.WriteLine($"Send message topic {topicName}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}