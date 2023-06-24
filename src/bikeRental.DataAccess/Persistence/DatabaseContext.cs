using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using bikeRental.Core.Common;
using bikeRental.Core.Entities;
using bikeRental.Core.Identity;
using bikeRental.Shared.Services;
using Microsoft.AspNetCore.Identity;
using bikeRental.DataAccess.Persistence.Configurations;
using System;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using bikeRental.Core.Enums;

namespace bikeRental.DataAccess.Persistence;

public class DatabaseContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
{
    private readonly IClaimService _claimService;

    public DatabaseContext(DbContextOptions<DatabaseContext> options, IClaimService claimService) : base(options)
    {
        _claimService = claimService;
    }

    public DbSet<TodoItem> TodoItems { get; set; }

    public DbSet<TodoList> TodoLists { get; set; }

    public DbSet<Bicycle> Bicycles { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Station> Stations { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);

        builder.Entity<Bicycle>()
                .HasOne(b => b.Station)
                .WithMany(s => s.Bicycles)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
        .OnDelete(DeleteBehavior.Cascade); 

        builder.Entity<Order>()
            .HasOne(o => o.Bicycle)
            .WithMany(b => b.Orders)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Station>()
            .HasIndex(s => s.Address)
            .IsUnique();

    }

    public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {

        foreach (var entry in ChangeTracker.Entries<IAuditedEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _claimService.GetUserId();
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedBy = _claimService.GetUserId();
                    entry.Entity.UpdatedOn = DateTime.UtcNow;
                    break;
            }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
