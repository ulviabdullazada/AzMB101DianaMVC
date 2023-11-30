using Diana.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diana.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using DianaDbContext context = new DianaDbContext();
            var sliders = await context.Sliders.ToListAsync();
            return View(sliders);
        }
    }
}
