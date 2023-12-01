using Diana.Models;
using Microsoft.EntityFrameworkCore;

namespace Diana.Contexts;

public class DianaDbContext : DbContext
{
    public DianaDbContext(DbContextOptions opt) : base(opt) { }
    public DbSet<Slider> Sliders { get; set; }
}
