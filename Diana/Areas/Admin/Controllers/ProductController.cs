using Diana.Areas.Admin.ViewModels;
using Diana.Contexts;
using Diana.Helpers;
using Diana.Models;
using Diana.ViewModels.ProductVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diana.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        DianaDbContext _db { get; }
        IWebHostEnvironment _env { get; }

        public ProductController(DianaDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index()
        {

            return View(_db.Products.Select(p=> new AdminProductListItemVM{
                Id= p.Id,
                Name= p.Name,
                CostPrice= p.CostPrice,
                Discount = p.Discount,
                Category = p.Category,
                ImageUrl = p.ImageUrl,
                IsDeleted = p.IsDeleted,
                Quantity = p.Quantity,
                SellPrice = p.SellPrice
            }));
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (!vm.ImageFile.IsCorrectType()) 
            {
                ModelState.AddModelError("ImageFile", "Wrong file type");
            }
            if (!vm.ImageFile.IsValidSize())
            {
                ModelState.AddModelError("ImageFile", "Files length must be less than kb");
            }
            if (vm.CostPrice > vm.SellPrice)
            {
                ModelState.AddModelError("CostPrice","Sell price must be bigger than cost price");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _db.Categories;
                return View(vm);
            }
            if (!await _db.Categories.AnyAsync(c=> c.Id == vm.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category doesnt exist");
                ViewBag.Categories = _db.Categories;
                return View(vm);
            }
            Product prod = new Product
            {
                Name = vm.Name,
                About = vm.About,
                Quantity = vm.Quantity,
                Description = vm.Description,
                Discount = vm.Discount,
                ImageUrl = await vm.ImageFile.SaveAsync(PathConstants.Product),
                CostPrice = vm.CostPrice,
                SellPrice = vm.SellPrice,
                CategoryId = vm.CategoryId
            };
            await _db.Products.AddAsync(prod);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
