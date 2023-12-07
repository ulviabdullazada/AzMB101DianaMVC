using System.ComponentModel.DataAnnotations;

namespace Diana.ViewModels.ColorVM
{
    public class ColorCreateVM
    {
        [Required, MaxLength(32)]
        public string Name { get; set; }
        [Required, MaxLength(7), MinLength(7)]
        public string HexCode { get; set; }
    }
}
