using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bikeRental.Core.Entities;

namespace bikeRental.DataAccess.Repositories;
public interface IBicycleRepository<TEntity> where TEntity : Bicycle
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetByStation(Guid? StationId);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> GetByIdAsync(Guid? id);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
}

