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

}

