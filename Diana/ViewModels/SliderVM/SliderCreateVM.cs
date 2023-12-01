using System.ComponentModel.DataAnnotations;

namespace Diana.ViewModels.SliderVM;

public class SliderCreateVM
{
    public string ImageUrl { get; set; }

    [Required, MinLength(3, ErrorMessage = "Başlığın uzunluğu ən az 3 simvoldan ibarət olmalıdır."), MaxLength(64), DataType("nvarchar")]
    public string Title { get; set; }

    [Required, MinLength(3), MaxLength(128), DataType("varchar")]
    public string Text { get; set; }
    [Required]
    public sbyte Position { get; set; }
}
