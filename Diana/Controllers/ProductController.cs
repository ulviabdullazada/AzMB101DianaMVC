using Diana.Areas.Admin.ViewModels;
using Diana.Contexts;
using Diana.Helpers;
using Diana.ViewModels.BasketVM;
using Diana.ViewModels.CommonVM;
using Diana.ViewModels.ProductVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Diana.Controllers
{
    public class ProductController : Controller
    {
        DianaDbContext _db { get; }

        public ProductController(DianaDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string? q, List<int>? catIds,List<int>? colorIds)
        {
            ViewBag.Categories = _db.Categories.Include(c=>c.Products);
            ViewBag.Colors = _db.Colors;
            var query = _db.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(p => p.Name.Contains(q));
            }
            if (catIds != null && catIds.Any())
            {
                query = query.Where(p => catIds.Contains(p.CategoryId));
            }
            if (colorIds != null && colorIds.Any())
            {
                var prodIds = _db.ProductColors.Where(c => colorIds.Contains(c.ColorId)).Select(c=>c.ProductId).AsQueryable();
                query = query.Where(p => prodIds.Contains(p.Id));
            }
            return View(query.Select(p => new AdminProductListItemVM
            {
                Id = p.Id,
                Category = p.Category,
                Discount = p.Discount,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                SellPrice = p.SellPrice
            }));
        }
        public async Task<IActionResult> ProductFilter(string? q, List<int>? catIds, List<int>? colorIds, int page = 1, int take = 8)
        {
            var query = _db.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(p => p.Name.Contains(q));
            }
            if (catIds != null && catIds.Any())
            {
                query = query.Where(p => catIds.Contains(p.CategoryId));
            }
            if (colorIds != null && colorIds.Any())
            {
                var prodIds = _db.ProductColors.Where(c => colorIds.Contains(c.ColorId)).Select(c => c.ProductId).AsQueryable();
                query = query.Where(p => prodIds.Contains(p.Id));
            }
            int count = await query.CountAsync();
            PaginatonVM<IEnumerable<AdminProductListItemVM>> pag = new PaginatonVM<IEnumerable<AdminProductListItemVM>>(count, page, (int)Math.Ceiling((decimal)count / take), query.AddPagination(page, take).Select(p => new AdminProductListItemVM
            {
                Id = p.Id,
                Category = p.Category,
                Discount = p.Discount,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                SellPrice = p.SellPrice
            }));
            return PartialView("_ProductPaginationPartial", pag);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Products.Select(p => new ProductDetailVM
            {
                Discount = p.Discount,
                Category = p.Category,
                Colors = p.ProductColors.Select(p => p.Color),
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                ImageUrls = p.ProductImages.Select(p => p.ImageUrl),
                Name = p.Name,
                Quantity = p.Quantity,
                Description = p.Description,
                About = p.About,
                SellPrice = (p.SellPrice - (p.SellPrice * (decimal)p.Discount)/ 100).ToString("0.##"),
            }).SingleOrDefaultAsync(p => p.Id == id);
            if (data == null) return NotFound();
            return View(data);
        }
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!await _db.Products.AnyAsync(p=> p.Id == id)) return NotFound();
            var basket = JsonConvert.DeserializeObject<List<BasketProductAndCountVM>>(HttpContext.Request.Cookies["basket"]??"[]");
            var existItem = basket.Find(b=>b.Id == id);
            if (existItem == null)
            {
                basket.Add(new BasketProductAndCountVM
                {
                    Id = (int)id,
                    Count = 1
                });
            }
            else
            {
                existItem.Count++;
            }
            HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket), new CookieOptions
            {
                MaxAge = TimeSpan.MaxValue
            });
            return Ok();
        }
    }
}
