using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MVC.Azure.Demo.Datas;
using MVC.Azure.Demo.Models.Admin;
using MVC.Azure.Demo.Services;

namespace MVC.Azure.Demo.Controllers.Admin;

public class JobController: Controller
{
    private readonly IBlobStorageService _iBlobStorageService;
    private readonly AppDbContext _appDbContext;
    private readonly IServiceBusSender _iServiceBusSender;
    public JobController(IBlobStorageService iBlobStorageService, AppDbContext appDbContext, IServiceBusSender iServiceBusSender)
    {
        _iBlobStorageService = iBlobStorageService;
        _appDbContext = appDbContext;
        _iServiceBusSender = iServiceBusSender;
    }
    
    [HttpPost]
    public async Task<ActionResult> Create(
        IFormFile formFile)
    {
        try
        {
            Image image = new Image(); 
            var id = Guid.NewGuid();
            image.Id = id;

            if (formFile?.Length > 0)
            {
                image.Name = formFile.FileName;
                image.Extention = formFile.ContentType;
                image.Size = formFile.Length;
                image.CreateDate = DateTime.Now;
                image.IsDelete = false;
                image.CreateBy = "admin";
                image.BlogName = await _iBlobStorageService.UploadBlob(formFile,image.Id.ToString());
                await _appDbContext.AddAsync(image);
                await _appDbContext.SaveChangesAsync();
                await _iServiceBusSender.SendMessage(new ImageForServiceBusModel()
                {
                    ImageId = image.Id,
                    BlogName = image.BlogName 
                });
            }
            else
            {
                throw new Exception("Tệp lỗi, Vui lòng kiểm tra lại");
            }
            return RedirectToAction(nameof(Index));
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
        return View();
    }
    
    public async Task<ActionResult> Index()
    {
        var data = (await _appDbContext.Images.Where(x=>!x.IsDelete).ToListAsync());
        data = data.OrderByDescending(x => x.CreateDate).ToList();
        List<ImageModel> res = new List<ImageModel>();
        res = data.Select(x => new ImageModel()
        {
            Id = x.Id,
            Extention = x.Extention,
            Name = x.Name,
            Size = x.Size,
            CreateBy = x.CreateBy,
            CreateDate = x.CreateDate,
            IsDelete = x.IsDelete,
            LastModifyTime = x.LastModifyTime,
            Type = x.Type,
            url = null,
            BlogName = x.BlogName
            
        }).ToList();
        foreach (var item in res)
        {
            if (!item.BlogName.IsNullOrEmpty())
            {
                item.url = await _iBlobStorageService.GetBlobUrl(item.BlogName);
            }
        }
        return View(res);
    }

    // public async Task<ActionResult> Details(string id, string industry)
    // {
    //     var data = await _tableStorageService.GetAttendee(industry, id);
    //     data.ImageName = await _blobStorageService.GetBlobUrl(data.ImageName);
    //     return View(data);
    // }
    //
    
    public ActionResult Create()
    {
        return View();
    }
}