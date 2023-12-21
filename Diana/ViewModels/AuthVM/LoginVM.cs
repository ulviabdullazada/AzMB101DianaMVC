using System.ComponentModel.DataAnnotations;

namespace Diana.ViewModels.AuthVM
{
    public class LoginVM
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemember { get; set; }
    }
}
