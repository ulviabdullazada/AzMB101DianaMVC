using Diana.Areas.Admin.ViewModels;
using Diana.ViewModels.CommonVM;
using Diana.ViewModels.SliderVM;

namespace Diana.ViewModels.HomeVM
{
    public class HomeVM
    {
        public IEnumerable<AdminProductListItemVM> Products { get; set; }
        public PaginatonVM<IEnumerable<AdminProductListItemVM>> PaginatedProducts { get; set; }
    }
}
