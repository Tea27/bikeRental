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
    Task<IEnumerable<BicycleModel>> GetAllAsync();
    Task<IEnumerable<BicycleModel>> GetByStation(Guid StationId);

    Task<BicycleModel> GetByIdAsync(Guid? id, Guid stationId);

    Task<BicycleModel> GetByIdAsync(Guid? id);

    Task Delete(Guid Id, Guid stationId);

    Task<BicycleModel> AddAsync(BicycleModel entity, Guid stationId);

    Task UpdateAsync(BicycleModel bicycleModel);

    IEnumerable<BicycleModel> SearchSelection(string searchString);
    IEnumerable<BicycleModel> SearchSelection(Guid Id, string searchString);

    IEnumerable<BicycleModel> FilterSelection(string filterString);
    IEnumerable<BicycleModel> FilterSelection(Guid Id, string filterString);
}

