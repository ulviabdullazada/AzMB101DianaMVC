using Diana.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Diana.ViewModels.ProductVM
{
    public class ProductDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SellPrice { get; set; }
        public float Discount { get; set; }
        public ushort Quantity { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<string> ImageUrls { get; set; }
        public Category? Category { get; set; }
        public IEnumerable<Color> Colors { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
    }
}
