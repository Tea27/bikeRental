using bikeRental.Core.Entities;
using bikeRental.DataAccess.Persistence;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace bikeRental.DataAccess.Repositories.Impl;
public class StationRepository<TEntity> : IStationRepository<TEntity> where TEntity : Station
{
    protected readonly DatabaseContext _context;
    protected readonly DbSet<TEntity> DbSet;

    public StationRepository(DatabaseContext context)
    {
        _context = context;
        DbSet = context.Set<TEntity>();
    }
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        entity.Id = Guid.NewGuid();

        var addedEntity = (await DbSet.AddAsync(entity)).Entity;
        await _context.SaveChangesAsync();
        return addedEntity;
    }
    public async Task<TEntity> GetByIdAsync(Guid? id)
    {
        return await FindByCondition(station => station.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
    {
        return DbSet.Where(expression).AsNoTracking();
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await DbSet.ToListAsync();

    }

    public async Task UpdateAsync(TEntity entity)
    {
        try
        {
            _context.Attach(entity).State = EntityState.Modified;
        }
        catch (DbUpdateException ex)
        {
            System.Diagnostics.Debug.WriteLine("\n\n\nOvoo je u bazi Attac" + ex + "\n\n\n");
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
