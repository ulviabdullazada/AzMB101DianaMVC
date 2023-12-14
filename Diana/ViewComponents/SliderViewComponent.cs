using Diana.Contexts;
using Diana.ViewModels.SliderVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diana.ViewComponents;
public class SliderViewComponent:ViewComponent
{
    DianaDbContext _context { get; }

    public SliderViewComponent(DianaDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _context.Sliders.Select(s => new SliderListItemVM
        {
            Id = s.Id,
            ImageUrl = s.ImageUrl,
            IsLeft = s.IsLeft,
            Title = s.Title,
            Text = s.Text,
        }).ToListAsync());
    }
}
