using System;

namespace ServiceBus.Queue.Trigger.ImageResize.DB.Entity;

public class Image
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string BlogName { get; set; }
    public string Extention { get; set; }
    public double Size { get; set; }
    public int? Type { get; set; }
    public string CreateBy { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? LastModifyTime { get; set; }
    public bool IsDelete { get; set; }
    public int Status { get; set; }
}