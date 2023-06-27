using bikeRental.Core.Entities;
using bikeRental.Core.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.DataAccess.Repositories;
public interface IUserRepository<TEntity> where TEntity : ApplicationUser
{
    IQueryable<TEntity> GetAll();

    Task<TEntity> GetByIdAsync(Guid? id);

    Task UpdateAsync(TEntity entity, string newRole);

    Task DeleteAsync(Guid id);

    Task AddAsync(TEntity entity, string role, string password);
    Task<TEntity> GetByIdAsyncIncludeOrders(Guid? id);
    IQueryable<TEntity> FindByCondition(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> expression);
}

