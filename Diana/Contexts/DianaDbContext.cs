using Diana.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diana.Contexts;

public class DianaDbContext : IdentityDbContext
{
    public DianaDbContext(DbContextOptions opt) : base(opt) { }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<ProductColor> ProductColors { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Setting>()
            .HasData(new Setting
            {
                Address = "Baku, Ayna Sultanova st. 221",
                Email = "royal234@mail.ru",
                Number1 = "+994707094535",
                Number2 = "+994773755354",
                Logo = "assets/img/logo.png",
                AccountIcon = "<i class='fa fa-user-o'></i>",
                Id = 1
            });
        base.OnModelCreating(modelBuilder);
    }
}
