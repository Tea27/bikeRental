using Microsoft.AspNetCore.Identity;
using bikeRental.Core.Identity;
using bikeRental.Core.Entities;
using bikeRental.Core.Enums;
using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Converters;

namespace bikeRental.DataAccess.Persistence;

public static class DatabaseContextSeed
{
    public static async Task SeedDatabaseAsync(DatabaseContext context, RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore)
    {

        string path = @"Seed";
        List<Station> stations = new List<Station>();

        if (!roleManager.Roles.Any())
        {
            foreach (var name in Enum.GetNames(typeof(Role)))
            {
                await roleManager.CreateAsync(new ApplicationRole(name));
            }
        }

        if (!userManager.Users.Any())
        {
            var emailStore = (IUserEmailStore<ApplicationUser>)userStore;
            var user = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com", EmailConfirmed = true, FirstName = "Admin", LastName = "Admin", Status = AccountStatus.Active};
            await userStore.SetUserNameAsync(user, user.Email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);

            await userManager.CreateAsync(user, "Admin123!");

            await userManager.AddToRoleAsync(user, "Administrator");
        }

        if (!context.Stations.Any())
        {

            var stationsJson = File.ReadAllText(path + Path.DirectorySeparatorChar + "stations.json");
            stations = JsonConvert.DeserializeObject<List<Station>>(stationsJson);
            await context.Stations.AddRangeAsync(stations);
            await context.SaveChangesAsync();

        }

        await context.SaveChangesAsync();
    }


}
