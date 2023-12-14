using Diana.Contexts;
using Diana.Models;
using Microsoft.EntityFrameworkCore;

namespace Diana.Helpers;

public class LayoutService
{
    DianaDbContext _context { get; }

    public LayoutService(DianaDbContext context)
    {
        _context = context;
    }

    public async Task<Setting> GetSettingsAsync()
        => await _context.Settings.FindAsync(1);
}
