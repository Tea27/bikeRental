
using bikeRental.Application.Models.Station;
using bikeRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Application.Services;
public interface IStationService
{
    Task<StationModel> AddAsync(StationModel entity);
    Task<StationModel> GetByIdAsync(Guid? id);
    IEnumerable<StationResponse> GetAll();
    Task UpdateAsync(StationModel stationModel);
    Task Delete(Guid Id);
    string GetAddressess();
    IEnumerable<StationResponse> CheckSwitch(string searchString, string sortOrder);
    string SaveError(Guid? id);
    Task DisableBicycles(Guid id);
    bool HasOrders(Guid? id);
}

