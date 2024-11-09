namespace MVC.Azure.Demo.Services;

public interface IImageService
{
    Task ImageResizeAsync(string blogName);
}