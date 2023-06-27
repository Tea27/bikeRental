using bikeRental.Application.Models.Bicycle;
using bikeRental.Application.Models.Station;
using bikeRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Application.Services;
public interface IBicycleService
{
    Task<BicycleModel> GetByIdAsync(Guid? id);

    Task Delete(Guid id);

    Task<BicycleModel> AddAsync(BicycleModel entity, Guid stationId);

    Task UpdateAsync(BicycleModel bicycleModel);

    IEnumerable<BicycleModel> CheckSwitch(string filterString, string searchString, string sortOrder, Guid? Id = null);
    Task<BicycleModel> GetByIdAsyncIncludeOrders(Guid? id);

    }

