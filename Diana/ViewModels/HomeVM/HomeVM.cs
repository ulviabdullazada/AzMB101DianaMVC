using Diana.Areas.Admin.ViewModels;
using Diana.ViewModels.SliderVM;

namespace Diana.ViewModels.HomeVM
{
    public class HomeVM
    {
        public IEnumerable<SliderListItemVM> Sliders { get; set; }
        public IEnumerable<AdminProductListItemVM> Products { get; set; }
    }
}
