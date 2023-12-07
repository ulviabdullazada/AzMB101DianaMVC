using System.ComponentModel.DataAnnotations;

namespace Diana.Models
{
    public class Color
    {
        public int Id { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(6), MinLength(3)]
        public string Hexcode { get; set; }
        public ICollection<ProductColor>? ProductColors { get; set; }
    }

}