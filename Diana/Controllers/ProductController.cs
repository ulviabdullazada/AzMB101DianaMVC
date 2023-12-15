using Diana.Contexts;
using Diana.ViewModels.BasketVM;
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

        public IActionResult Index()
        {
            return View();
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
