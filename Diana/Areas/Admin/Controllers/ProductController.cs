using Diana.Areas.Admin.ViewModels;
using Diana.Contexts;
using Diana.Helpers;
using Diana.Models;
using Diana.ViewModels.CommonVM;
using Diana.ViewModels.ProductVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

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

        public async Task<IActionResult> Index()
        {
            int total = await _db.Products.CountAsync();
            var data = new PaginatonVM<IEnumerable<AdminProductListItemVM>>(
                total,
                1, 
                (int)Math.Ceiling((decimal)total / 8), 
                await _db.Products.AddPagination(1, 8).Select(p => new AdminProductListItemVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    CostPrice = p.CostPrice,
                    Discount = p.Discount,
                    Category = p.Category,
                    ImageUrl = p.ImageUrl,
                    IsDeleted = p.IsDeleted,
                    Quantity = p.Quantity,
                    SellPrice = p.SellPrice,
                    Colors = p.ProductColors.Select(pc => pc.Color)
                }).ToListAsync());
            
            return View(data);
        }
        public async Task<IActionResult> ProductPagination(int page, int count)
        {
            int total = await _db.Products.CountAsync();
            var data = new PaginatonVM<IEnumerable<AdminProductListItemVM>>(
                total,
                page,
                (int)Math.Ceiling((decimal)total / count),
                await _db.Products.AddPagination(page, count).Select(p => new AdminProductListItemVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    CostPrice = p.CostPrice,
                    Discount = p.Discount,
                    Category = p.Category,
                    ImageUrl = p.ImageUrl,
                    IsDeleted = p.IsDeleted,
                    Quantity = p.Quantity,
                    SellPrice = p.SellPrice,
                    Colors = p.ProductColors.Select(pc => pc.Color)
                }).ToListAsync());
            return PartialView("_PaginationProductPartial",data);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories;
            ViewBag.Colors = new SelectList(_db.Colors,"Id","Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (vm.ImageFile != null)
            {
                if (!vm.ImageFile.IsCorrectType())
                {
                    ModelState.AddModelError("ImageFile", "Wrong file type");
                }
                if (!vm.ImageFile.IsValidSize())
                {
                    ModelState.AddModelError("ImageFile", "Files length must be less than kb");
                }
            }
            if (vm.Images != null)
            {
                //string message = string.Empty;
                foreach (var img in vm.Images)
                {
                    if (!img.IsCorrectType())
                    {
                        ModelState.AddModelError("", "Wrong file type (" + img.FileName + ")");
                        //message += "Wrong file type (" + img.FileName + ") \r\n";
                    }
                    if (!img.IsValidSize(200))
                    {
                        ModelState.AddModelError("", "Files length must be less than kb (" + img.FileName + ")");
                        //message += "Files length must be less than kb (" + img.FileName + ") \r\n";
                    }
                }
                //if (!string.IsNullOrEmpty(message))
                //{
                //    ModelState.AddModelError("Images", message);
                //}
            }
            if (vm.CostPrice > vm.SellPrice)
            {
                ModelState.AddModelError("CostPrice","Sell price must be bigger than cost price");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _db.Categories;
                ViewBag.Colors = new SelectList(_db.Colors,"Id","Name");
                return View(vm);
            }
            if (!await _db.Categories.AnyAsync(c=> c.Id == vm.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category doesnt exist");
                ViewBag.Categories = _db.Categories;
                ViewBag.Colors = new SelectList(_db.Colors,"Id","Name");
                return View(vm);
            }
            //var a = await (from c in _db.Colors
            //               where vm.ColorIds.Contains(c.Id)
            //               select c.Id).CountAsync();
            if (await _db.Colors.Where(c=>vm.ColorIds.Contains(c.Id)).Select(c=>c.Id).CountAsync() != vm.ColorIds.Count())
            {
                ModelState.AddModelError("ColorIds", "Color doesnt exist");
                ViewBag.Categories = _db.Categories;
                ViewBag.Colors = new SelectList(_db.Colors, "Id", "Name");
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
                CategoryId = vm.CategoryId,
                ProductColors = vm.ColorIds.Select(id=> new ProductColor
                {
                    ColorId = id,
                }).ToList(),
                ProductImages = vm.Images.Select(i => new ProductImage
                {
                    ImageUrl = i.SaveAsync(PathConstants.Product).Result
                }).ToList()
            };
            //IList<ProductColor> list = new List<ProductColor>();
            //foreach (var id in vm.ColorIds)
            //{
            //    list.Add(new ProductColor
            //    {
            //        ColorId = id,
            //        Product = prod
            //    });
            //}
            //await _db.ProductColors.Add(list);
            await _db.Products.AddAsync(prod);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            ViewBag.Colors = new SelectList(_db.Colors, nameof(Color.Id), nameof(Color.Name));
            ViewBag.Categories = new SelectList(_db.Categories, nameof(Category.Id), nameof(Category.Name));
            var data = await _db.Products
                .Include(p => p.ProductImages)
                .Include(p=>p.ProductColors)
                .SingleOrDefaultAsync(p=> p.Id == id);
                //.Include(p => p.ProductColors)
                //    .ThenInclude(pc => pc.Color)
                //.Include(p => p.Category)
            if (data == null) return NotFound();

            var vm = new ProductUpdateVM
            {
                About = data.About,
                CategoryId = data.CategoryId,
                ColorIds = data.ProductColors?.Select(i => i.ColorId),
                CostPrice = data.CostPrice,
                Description = data.Description,
                Discount = data.Discount,
                Name = data.Name,
                Quantity = data.Quantity,
                SellPrice = data.SellPrice,
                ImageUrls = data.ProductImages?.Select(pi =>new ProductImageVM{
                    Id = pi.Id,
                    Url = pi.ImageUrl
                }),
                CoverImageUrl = data.ImageUrl
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (vm.ImageFile != null)
            {
                if (!vm.ImageFile.IsCorrectType())
                {
                    ModelState.AddModelError("ImageFile", "Wrong file type");
                }
                if (!vm.ImageFile.IsValidSize())
                {
                    ModelState.AddModelError("ImageFile", "Files length must be less than kb");
                }
            }
            if (vm.Images != null)
            {
                //string message = string.Empty;
                foreach (var img in vm.Images)
                {
                    if (!img.IsCorrectType())
                    {
                        ModelState.AddModelError("", "Wrong file type (" + img.FileName + ")");
                        //message += "Wrong file type (" + img.FileName + ") \r\n";
                    }
                    if (!img.IsValidSize(200))
                    {
                        ModelState.AddModelError("", "Files length must be less than kb (" + img.FileName + ")");
                        //message += "Files length must be less than kb (" + img.FileName + ") \r\n";
                    }
                }
            }
            if (vm.CostPrice > vm.SellPrice)
            {
                ModelState.AddModelError("CostPrice", "Sell price must be bigger than cost price");
            }
           
            if (!vm.ColorIds.Any())
            {
                ModelState.AddModelError("ColorIds", "You must add at least 1 color");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Colors = new SelectList(_db.Colors, nameof(Color.Id), nameof(Color.Name));
                ViewBag.Categories = new SelectList(_db.Categories, nameof(Category.Id), nameof(Category.Name));
                return View(vm);
            }
           
            var data = await _db.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductColors)
                .SingleOrDefaultAsync(p => p.Id == id);
            if (data == null) return NotFound();

            if (vm.ImageFile != null)
            {
                string filePath = Path.Combine(PathConstants.RootPath, data.ImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                data.ImageUrl = await vm.ImageFile.SaveAsync(PathConstants.Product);
            }
            if (vm.Images != null)
            {
                var imgs = vm.Images.Select(i => new ProductImage
                {
                    ImageUrl = i.SaveAsync(PathConstants.Product).Result,
                    ProductId = data.Id
                });

                data.ProductImages.AddRange(imgs);
            }
            
            if (!Enumerable.SequenceEqual(data.ProductColors?.Select(p => p.ColorId), vm.ColorIds))
            {
                data.ProductColors = vm.ColorIds.Select(c => new ProductColor { ColorId = c, ProductId = data.Id }).ToList();
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteImageCSharp(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.ProductImages.FindAsync(id);
            if (data == null) return NotFound();
            _db.ProductImages.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Update), new { id = data.ProductId });
        }
        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.ProductImages.FindAsync(id);
            if (data == null) return NotFound();
            _db.ProductImages.Remove(data);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
