using MVC.Azure.Demo.Datas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using MVC.Azure.Demo.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("AzureSqlConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

var storageConnectionString = builder.Configuration["Azure:Storage:ConnectionString"];
builder.Services.AddAzureClients(builder =>
{
    builder.AddBlobServiceClient(storageConnectionString);
});
builder.Services.AddTransient<IImageService,ImageService>();
builder.Services.AddTransient<IBlobStorageService, BlobStorageService>();
builder.Services.AddTransient<IServiceBusSender,ServiceBusSender>();
builder.Services.AddControllersWithViews();
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();