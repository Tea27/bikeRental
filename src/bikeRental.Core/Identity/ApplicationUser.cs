using Microsoft.AspNetCore.Identity;
using bikeRental.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace bikeRental.Core.Identity;


public class ApplicationUser : IdentityUser<Guid>{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public ICollection<Order> Orders { get; set; }


}
