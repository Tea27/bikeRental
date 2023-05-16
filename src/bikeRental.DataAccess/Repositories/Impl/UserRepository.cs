using bikeRental.Core.Entities;
using bikeRental.Core.Identity;
using bikeRental.DataAccess.Persistence;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace bikeRental.DataAccess.Repositories.Impl;

public class UserRepository<TEntity> : IUserRepository<TEntity> where TEntity : ApplicationUser
{
    private readonly DatabaseContext _context;
    //private readonly UserManager<TEntity> _userManager;
    private readonly DbSet<TEntity> DbSet;
    public UserRepository(DatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        // _userManager = userManager;
        DbSet = context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await DbSet.ToListAsync();

    }

    public async Task<TEntity> GetByIdAsync(Guid? id)
    {
        return await FindByCondition(user => user.Id.Equals(id)).FirstOrDefaultAsync();
    }
    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
    {
        return DbSet.Where(expression).AsNoTracking();
    }
    public async Task UpdateAsync(TEntity entity)
    {
        try
        {
            _context.Attach(entity).State = EntityState.Modified;
        }
        catch (DbUpdateException ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }

        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id)
    {
        var station = new Station() { Id = id };
        _context.Stations.Remove(station);
        await _context.SaveChangesAsync();
    }
}

