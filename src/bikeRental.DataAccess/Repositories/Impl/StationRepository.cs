using bikeRental.Core.Entities;
using bikeRental.DataAccess.Persistence;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using bikeRental.Core.Enums;

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
        return await FindByCondition(station => station.Id.Equals(id)).SingleOrDefaultAsync();
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
    {
        return DbSet.Where(expression).Include(s => s.Bicycles).ThenInclude(b => b.Orders).AsNoTracking();
    }
    public IQueryable<TEntity> FindByCondition(IQueryable<TEntity> entity, Expression<Func<TEntity, bool>> expression)
    {
        return entity.Where(expression).AsNoTracking();
    }

    public async Task<TEntity> FindWithOrders(Guid Id)
    {
        return await DbSet.Where(station => station.Id == Id).Include(s => s.Bicycles).ThenInclude(b => b.Orders).AsNoTracking().FirstOrDefaultAsync();
    }
    public IQueryable<TEntity> GetAll()
    {
        return DbSet.AsQueryable();        
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
        var station = await GetByIdAsync(id);
        DbSet.Remove(station);
        await _context.SaveChangesAsync();
    }
}
