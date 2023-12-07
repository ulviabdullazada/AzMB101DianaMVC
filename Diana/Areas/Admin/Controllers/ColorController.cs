using Diana.Contexts;
using Diana.ViewModels.ColorVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diana.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        DianaDbContext _context { get; }

        public ColorController(DianaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Colors.Select(c=> new ColorListItemVM
            {
                Id = c.Id,
                Name = c.Name,
                HexCode = c.Hexcode
            }).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ColorCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _context.Colors.AddAsync(new Models.Color
            {
                Name = vm.Name,
                Hexcode = vm.HexCode.Substring(1)
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
