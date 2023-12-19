using Diana.Contexts;
using Diana.Models;
using Diana.ViewModels.SliderVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diana.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
    public class SliderController : Controller
    {
        DianaDbContext _db { get; }
        public SliderController(DianaDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            //var sliders = await db.Sliders.ToListAsync();
            //List<SliderListItemVM> items = new List<SliderListItemVM>();
            //foreach (var sItem in sliders)
            //{
            //    items.Add(new SliderListItemVM
            //    {
            //        Id = sItem.Id,
            //        ImageUrl = sItem.ImageUrl,
            //        IsLeft = sItem.IsLeft,
            //        Text = sItem.Text,
            //        Title = sItem.Title,
            //    });
            //}
            var items = await _db.Sliders.Select(s => new SliderListItemVM
            {
                Title = s.Title,
                Text = s.Text,
                IsLeft = s.IsLeft,
                ImageUrl = s.ImageUrl,
                Id = s.Id,
            }).ToListAsync();
            return View(items);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM vm)
        {
            if (vm.Position < -1 || vm.Position > 1)
            {
                ModelState.AddModelError("Position", "Wrong input");
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Slider slider = new Slider
            {
                Title = vm.Title,
                Text = vm.Text,
                ImageUrl = vm.ImageUrl,
                IsLeft = vm.Position switch
                {
                    0 => null,
                    -1 => true,
                    1 => false
                }
            };
            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["Response"] = false;
            if (id == null) return BadRequest();
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            _db.Sliders.Remove(data);
            await _db.SaveChangesAsync();
            TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            return View(new SliderUpdateVM { 
                ImageUrl = data.ImageUrl,
                Position = data.IsLeft switch
                {
                    true => -1,
                    null => 0,
                    false => 1
                },
                Text = data.Text,
                Title = data.Title
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, SliderUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (vm.Position < -1 || vm.Position > 1)
            {
                ModelState.AddModelError("Position", "Wrong input");
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            data.Text = vm.Text;
            data.Title = vm.Title;
            data.ImageUrl = vm.ImageUrl;
            data.IsLeft = vm.Position switch
            {
                0 => null,
                -1 => true,
                1 => false
            };
            await _db.SaveChangesAsync();
            TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}
