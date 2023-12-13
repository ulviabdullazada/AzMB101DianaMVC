using Diana.Areas.Admin.ViewModels;
using Diana.Contexts;
using Diana.ViewModels.CommonVM;
using Diana.ViewModels.HomeVM;
using Diana.ViewModels.SliderVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            int take = 4;
            var items = _context.Products.Where(p => !p.IsDeleted).Take(take).Select(p => new AdminProductListItemVM
            {
                Id = p.Id,
                Category = p.Category,
                Discount = p.Discount,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                Quantity = p.Quantity,
                SellPrice = p.SellPrice
            });
            int count = await _context.Products.CountAsync(x=>!x.IsDeleted);
            PaginatonVM<IEnumerable<AdminProductListItemVM>> pag = new(count, 1, (int)Math.Ceiling((decimal)count / take), items);
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
                }).ToListAsync(),
                PaginatedProducts = pag
            };
            return View(vm);
        }
        public async Task<IActionResult> ProductPagination(int page = 1, int count = 8)
        {
            var items = _context.Products.Where(p => !p.IsDeleted).Skip((page-1)*count).Take(count).Select(p => new AdminProductListItemVM
            {
                Id = p.Id,
                Category = p.Category,
                Discount = p.Discount,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                Quantity = p.Quantity,
                SellPrice = p.SellPrice
            });
            int totalCount = await _context.Products.CountAsync(x => !x.IsDeleted);
            PaginatonVM<IEnumerable<AdminProductListItemVM>> pag = new(totalCount, page, (int)Math.Ceiling((decimal)totalCount / count), items);
            
            return PartialView("_ProductPaginationPartial", pag);
        }
    }
}
