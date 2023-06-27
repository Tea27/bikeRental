using bikeRental.Application.Models.Order;
using bikeRental.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace bikeRental.Application.Models.User;

public class UserModel : BaseResponseModel
{

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public Role Role { get; set; }

    public AccountStatus Status { get; set; }
    public ICollection<OrderModel> Orders { get; set; }
}
