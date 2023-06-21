using Microsoft.AspNetCore.Identity;
using bikeRental.Core.Entities;
using bikeRental.Core.Enums;

namespace bikeRental.Core.Identity;


public class ApplicationUser : IdentityUser<Guid>{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public AccountStatus Status { get; set; }

    public ICollection<Order> Orders { get; set; }


}
