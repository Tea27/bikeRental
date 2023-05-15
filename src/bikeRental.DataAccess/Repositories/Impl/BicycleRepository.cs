using bikeRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bikeRental.DataAccess.Repositories;
using bikeRental.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

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


}

