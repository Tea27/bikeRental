using bikeRental.Core.Entities;
using bikeRental.Core.Identity;
using Microsoft.AspNetCore.Identity;
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

    Task UpdateAsync(TEntity entity, string newRole);

    Task DeleteAsync(Guid id);

    Task AddAsync(TEntity entity, string role, string password);

}

