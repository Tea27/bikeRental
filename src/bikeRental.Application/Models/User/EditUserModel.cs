using bikeRental.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace bikeRental.Application.Models.User;
public class EditUserModel : BaseResponseModel
{
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "First name")]
    public string FirstName { get; set; }

    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Last name")]
    public string LastName { get; set; }

    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Username")]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "User role")]
    public Role Role { get; set; }

    [Required]
    [Display(Name = "Account status")]
    public AccountStatus Status { get; set; }
}
