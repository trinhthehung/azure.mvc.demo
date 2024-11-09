namespace MVC.Azure.Demo.Datas;

public class ImageEx
{
    public Guid Id { get; set; }
    public Guid? ImageId { get; set; }
    public string Name { get; set; }
    public string BlogName { get; set; }
    public string Extention { get; set; }
    public double Size { get; set; }
    public string ReSize { get; set; }
    public int? Type { get; set; }
    public string CreateBy { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? LastModifyTime { get; set; }
    public bool IsDelete { get; set; }
}