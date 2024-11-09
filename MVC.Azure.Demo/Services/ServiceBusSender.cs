using System.Text;
using Microsoft.Azure.ServiceBus;
using MVC.Azure.Demo.Models.Admin;
using Newtonsoft.Json;

namespace MVC.Azure.Demo.Services;

public class ServiceBusSender:IServiceBusSender
{
    private readonly QueueClient _queueClient;
    private readonly IConfiguration _configuration;

    public ServiceBusSender(IConfiguration configuration)
    {
        _configuration = configuration;
        _queueClient = new QueueClient(_configuration["Azure:ServiceBus:ConnectionString"], _configuration["Azure:ServiceBus:ImageQueueName"]);
    }
    
    public async Task SendMessage(ImageForServiceBusModel payload)
    {
        try
        {
            
            string data = JsonConvert.SerializeObject(payload);
            Message message = new Message(Encoding.UTF8.GetBytes(data));
            await _queueClient.SendAsync(message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}