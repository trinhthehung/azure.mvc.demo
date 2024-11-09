using System;

namespace ServiceBus.Queue.Trigger.ImageResize.Dto;

public class MessageTopicImageResizeInfoDto
{
    public Guid Id { get; set; }
    public Guid ImageId { get; set; }
}