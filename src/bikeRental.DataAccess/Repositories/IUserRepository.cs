using bikeRental.Core.Entities;
using bikeRental.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.DataAccess.Repositories;
public interface IUserRepository<TEntity> where TEntity : ApplicationUser
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity> GetByIdAsync(Guid? id);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(Guid id);
}

