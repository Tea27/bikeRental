using bikeRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.DataAccess.Repositories;
public interface IStationRepository<TEntity> where TEntity : Station
{
    Task<TEntity> AddAsync(TEntity entity);
    IQueryable<TEntity> GetAll();
    Task<TEntity> GetByIdAsync(Guid? id);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression);
    IQueryable<TEntity> FindByCondition(IQueryable<TEntity> entity, Expression<Func<TEntity, bool>> expression);
}
