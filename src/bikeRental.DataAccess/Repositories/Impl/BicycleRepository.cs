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
using static System.Collections.Specialized.BitVector32;

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

    public async Task<TEntity> AddAsync(TEntity entity, Guid stationId)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        entity.Id = Guid.NewGuid();

        // Retrieve the associated Station entity from the database using its StationId
        var associatedStation = await _context.Stations.FindAsync(stationId);
        if (associatedStation == null)
        {
            // Handle the situation when the associated Station does not exist
            throw new InvalidOperationException("The associated Station does not exist.");
        }

        // Associate the Bicycle entity with the existing Station entity
        entity.Station = associatedStation;
        if (entity.Type.ToString() == "Acoustic")
        {
            entity.Station.NumberOfBikes++;
        }
        else
        {
            entity.Station.NumberOfElectricBikes++;
        }

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
    public async Task UpdateAsync(TEntity entity, Guid stationId)
    {
        entity.Station = await _context.Stations.FindAsync(stationId);
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

