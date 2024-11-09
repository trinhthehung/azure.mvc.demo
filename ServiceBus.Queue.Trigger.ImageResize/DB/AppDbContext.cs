using Microsoft.EntityFrameworkCore;
using ServiceBus.Queue.Trigger.ImageResize.DB.Entity;

namespace ServiceBus.Queue.Trigger.ImageResize.DB;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public AppDbContext()
    {
    }
    public DbSet<Image> Image { get; set; }
    public DbSet<ImageEx> ImageEx { get; set; }
}