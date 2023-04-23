using Microsoft.AspNetCore.Identity;
using bikeRental.Core.Entities;

namespace bikeRental.Core.Identity;


public class ApplicationUser : IdentityUser<Guid>{
    public string FirstName { get; set; }

    public string LastName { get; set; }
}
