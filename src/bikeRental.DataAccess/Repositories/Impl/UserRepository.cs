using bikeRental.Core.Identity;
using bikeRental.DataAccess.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace bikeRental.DataAccess.Repositories.Impl;

public class UserRepository<TEntity> : IUserRepository<TEntity> where TEntity : ApplicationUser
{
    private readonly DatabaseContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly DbSet<TEntity> DbSet;
    private readonly IUserStore<ApplicationUser> _userStore;
    public UserRepository(DatabaseContext context, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userManager = userManager;
        DbSet = context.Set<TEntity>();
        _userStore = userStore;
    }

    public IQueryable<TEntity> GetAll()
    {
        return DbSet.AsQueryable();
    }

    public async Task<TEntity> GetByIdAsync(Guid? id)
    {
        return await FindByCondition(user => user.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
    {
        return DbSet.Where(expression).AsNoTracking();
    }

    public IQueryable<TEntity> FindByCondition(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> expression)
    {
        return query.Where(expression).AsNoTracking();
    }
    public async Task UpdateAsync(TEntity entity, string newRole)
    {
        
        try
        {
            var editItem = await _userManager.FindByIdAsync(entity.Id.ToString());

            editItem.FirstName = entity.FirstName;
            editItem.LastName = entity.LastName;
            editItem.Email = entity.Email;
            editItem.UserName = entity.Email;
            editItem.Status = entity.Status;

            await _userManager.UpdateAsync(editItem);

            var oldRole = _userManager.GetRolesAsync(editItem).Result.ToList().First();

            await _userStore.UpdateAsync(editItem, CancellationToken.None);
            await _userManager.RemoveFromRoleAsync(editItem, oldRole);
            await _userManager.AddToRoleAsync(editItem, newRole);
        }
        catch (DbUpdateException ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }
    public async Task DeleteAsync(Guid id)
    {
        var user = await GetByIdAsync(id);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(TEntity entity, string role, string password)
    {
        var user = new ApplicationUser { UserName = entity.Email, FirstName = entity.FirstName, LastName = entity.LastName, EmailConfirmed = true, Email = entity.Email };
        try
        {
            var emailStore = (IUserEmailStore<ApplicationUser>)_userStore;

            await _userStore.SetUserNameAsync(user, user.Email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);

            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, role);
        }
        catch (DbUpdateException ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }
    public async Task<TEntity> GetByIdAsyncIncludeOrders(Guid? id)
    {
        return await FindByCondition(customer => customer.Id.Equals(id)).Include(c => c.Orders).SingleOrDefaultAsync();

    }

}

