namespace MVC.Azure.Demo.Services;

public interface IServiceBusConsumer
{
    void RegisterOnMessageHandlerAndReceiveMessages();

    Task CloseQueueAsync();
}