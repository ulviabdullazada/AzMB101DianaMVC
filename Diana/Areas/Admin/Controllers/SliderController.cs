using Diana.Contexts;
using Diana.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diana.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using DianaDbContext db = new DianaDbContext();
            return View(await db.Sliders.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View(slider);
            }
            using DianaDbContext db = new DianaDbContext();
            await db.Sliders.AddAsync(slider);
            await db.SaveChangesAsync();
            return View();
        }
    }
}
