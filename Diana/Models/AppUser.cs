using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diana.Models;

public class AppUser : IdentityUser
{
    public string Fullname { get; set; }
    public string? ProfileImageUrl { get; set; }
    [NotMapped]
    public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
    [NotMapped]
    public override bool PhoneNumberConfirmed { get => base.PhoneNumberConfirmed; set => base.PhoneNumberConfirmed = value; }
}
