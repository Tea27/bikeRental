using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using bikeRental.Core.Entities;

namespace bikeRental.DataAccess.Repositories;
public interface IBicycleRepository<TEntity> where TEntity : Bicycle
{
    IQueryable<TEntity> GetAll();
    Task<TEntity> AddAsync(TEntity entity, Guid stationId);
    Task<TEntity> GetByIdAsync(Guid? id);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression);
    IQueryable<TEntity> FindByCondition(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> expression);
    Task<TEntity> GetByIdAsyncIncludeOrders(Guid? id);


}

