using System;

namespace ServiceBus.Queue.Trigger.ImageResize.Dto;

public class MessageQueueImageForServiceBusDto
{
    public Guid ImageId { get; set; }
    public string BlogName { get; set; }
}