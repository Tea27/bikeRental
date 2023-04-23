using bikeRental.DataAccess.Persistence;

namespace bikeRental.API;

public static class Program
{
    public static async Task Main(string[] args)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            await AutomatedMigration.MigrateAsync(scope.ServiceProvider);
        }

        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
