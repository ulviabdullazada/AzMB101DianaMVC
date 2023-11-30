using Diana.Models;
using Microsoft.EntityFrameworkCore;

namespace Diana.Contexts;

public class DianaDbContext : DbContext
{
    public DbSet<Slider> Sliders { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-7549KO9\\SQLEXPRESS;Database = Diana;Trusted_Connection=true");
        base.OnConfiguring(optionsBuilder);
    }
}
