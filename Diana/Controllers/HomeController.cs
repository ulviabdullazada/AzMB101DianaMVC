using Diana.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diana.Controllers
{
    public class HomeController : Controller
    {
        DianaDbContext _context { get; }

        public HomeController(DianaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _context.Sliders.ToListAsync();
            return View(sliders);
        }
    }
}
