using Diana.Areas.Admin.ViewModels;
using Diana.Contexts;
using Diana.ViewModels.HomeVM;
using Diana.ViewModels.SliderVM;
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
            HomeVM vm = new HomeVM
            {
                Sliders = await _context.Sliders.Select(s => new SliderListItemVM
                {
                    Id = s.Id,
                    ImageUrl = s.ImageUrl,
                    IsLeft = s.IsLeft,
                    Title = s.Title,
                    Text = s.Text,
                }).ToListAsync(),
                Products = await _context.Products.Where(p => !p.IsDeleted).Select(p => new AdminProductListItemVM {
                    Id = p.Id,
                    Category = p.Category,
                    Discount = p.Discount,
                    Name = p.Name,
                    ImageUrl= p.ImageUrl,
                    Quantity = p.Quantity,
                    SellPrice = p.SellPrice
                }).ToListAsync()
            };
            return View(vm);
        }
    }
}
