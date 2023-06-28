using bikeRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.DataAccess.Repositories
{
    public interface IOrderRepository<TEntity> where TEntity : Order
    {
        Task<TEntity> AddAsync(TEntity entity, Guid customerId, Guid bicycleId);
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetByIdAsync(Guid? id);
        Task UpdateAsync(TEntity entity, Guid customerId, Guid bicycleId);
        Task<IEnumerable<TEntity>> GetByCustomer(Guid? customerId);
        Task<IEnumerable<TEntity>> GetByBicycle(Guid? bicycleId);
    }
}
