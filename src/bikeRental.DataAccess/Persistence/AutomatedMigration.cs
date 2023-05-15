using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using bikeRental.Core.Identity;
using bikeRental.Core.Entities;

namespace bikeRental.DataAccess.Persistence;

public static class AutomatedMigration
{
    public static async Task MigrateAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<DatabaseContext>();

        if (context.Database.IsNpgsql()) await context.Database.MigrateAsync();

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

        var userStore = services.GetRequiredService<IUserStore<ApplicationUser>>();

        await DatabaseContextSeed.SeedDatabaseAsync(context, roleManager, userManager, userStore);
    }
}
