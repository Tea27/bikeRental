using bikeRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bikeRental.DataAccess.Repositories;
using bikeRental.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace bikeRental.DataAccess.Repositories.Impl;

public class BicycleRepository<TEntity> : IBicycleRepository<TEntity> where TEntity : Bicycle
{
    protected readonly DatabaseContext _context;
    protected readonly DbSet<TEntity> DbSet;

    public BicycleRepository(DatabaseContext context)
    {
        _context = context;
        DbSet = context.Set<TEntity>();
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await DbSet.ToListAsync();

    }
    public async Task<IEnumerable<TEntity>> GetByStation(Guid? stationId)
    {
        return await FindByCondition(TEntity => TEntity.Station.Id.Equals(stationId)).ToListAsync();
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
        return await FindByCondition(bicycle => bicycle.Id.Equals(id)).FirstOrDefaultAsync();

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
        var bicycle = new Bicycle() { Id = id };
        _context.Bicycles.Remove(bicycle);
        await _context.SaveChangesAsync();
    }


}

