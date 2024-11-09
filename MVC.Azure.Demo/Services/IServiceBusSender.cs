using MVC.Azure.Demo.Models.Admin;

namespace MVC.Azure.Demo.Services;

public interface IServiceBusSender
{
     Task SendMessage(ImageForServiceBusModel payload);
}