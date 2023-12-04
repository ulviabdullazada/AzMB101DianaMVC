using System.ComponentModel.DataAnnotations;

namespace Diana.ViewModels.CategoryVM
{
    public class CategoryCreateVM
    {
        [Required, MaxLength(16)]
        public string Name { get; set; }
    }
}
