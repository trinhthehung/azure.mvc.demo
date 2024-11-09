using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceBus.Queue.Trigger.ImageResize.DB;
using ServiceBus.Queue.Trigger.ImageResize.ServiceBus;

[assembly: FunctionsStartup(typeof(ServiceBus.Queue.Trigger.ImageResize.Startup))]
namespace ServiceBus.Queue.Trigger.ImageResize;

public class Startup:FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        string sqlConnectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
        var serviceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
        if (string.IsNullOrEmpty(serviceBusConnectionString))
        {
            throw new InvalidOperationException(
                "Please specify a valid ServiceBusConnectionString in the Azure Functions Settings or your local.settings.json file.");
        }
        builder.Services.AddSingleton((s) => {
            return new ServiceBusClient(serviceBusConnectionString
                , new ServiceBusClientOptions()
                {
                    TransportType = ServiceBusTransportType.AmqpWebSockets,
                }); 
        });
        builder.Services.AddDbContext<AppDbContext>(
            options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, sqlConnectionString));
        builder.Services.AddScoped<IPublishService, PublishService>();
        builder.Services.AddScoped<IBlobService, BlobService>();
    }
}