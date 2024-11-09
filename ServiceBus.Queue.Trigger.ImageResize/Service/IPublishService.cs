using System.Threading.Tasks;

namespace ServiceBus.Queue.Trigger.ImageResize.ServiceBus;

public interface IPublishService
{
    Task SensMessageAsync<T>(string topicName, T message) where T: class;
}