using bikeRental.Core.Entities;
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
    public IQueryable<TEntity> GetAll()
    {
        return DbSet.Include(b => b.Station).AsQueryable();
    }

    public async Task<TEntity> AddAsync(TEntity entity, Guid stationId)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        entity.Id = Guid.NewGuid();

        var station = await _context.Stations.FindAsync(stationId);
        if (station == null)
        {
            throw new InvalidOperationException("The associated Station does not exist.");
        }

        entity.Station = station;

        var addedEntity = (await DbSet.AddAsync(entity)).Entity;
        await _context.SaveChangesAsync();
        return addedEntity;
    }


    public async Task<TEntity> GetByIdAsync(Guid? id)
    {
        return await FindByCondition(bicycle => bicycle.Id.Equals(id)).SingleOrDefaultAsync();

    }
    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
    {
        return DbSet.Include(x => x.Station).Where(expression).AsNoTracking();
    }

    public IQueryable<TEntity> FindByCondition(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> expression)
    {
        return query.Where(expression).AsNoTracking();
    }
    public async Task UpdateAsync(TEntity entity)
    {
        try
        {
            var existingEntity = await DbSet.FindAsync(entity.Id);
            _context.Entry(existingEntity).State = EntityState.Detached;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();          
        }
        catch (DbUpdateException ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }
    public async Task DeleteAsync(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        _context.Entry(entity).State = EntityState.Detached;
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}

