using Microsoft.EntityFrameworkCore;
namespace MVC.Azure.Demo.Datas;

public class AppDbContext : DbContext 
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Image>(b =>
        {
            b.ToTable("Image");
            b.Property(x => x.Name).HasMaxLength(200);
            b.Property(x => x.BlogName).HasMaxLength(200);
            b.Property(x => x.Extention).HasMaxLength(10);
            b.Property(x => x.CreateBy).HasMaxLength(100);
            b.Property(x => x.Status).HasDefaultValue(1);
        });
        builder.Entity<ImageEx>(b =>
        {
            b.ToTable("ImageEx");
            b.Property(x => x.Name).HasMaxLength(200);
            b.Property(x => x.BlogName).HasMaxLength(200);
            b.Property(x => x.Extention).HasMaxLength(10);
            b.Property(x => x.CreateBy).HasMaxLength(100);
        });
    }
    public DbSet<Image> Images { get; set; }
    public DbSet<ImageEx> ImageExs { get; set; }
}