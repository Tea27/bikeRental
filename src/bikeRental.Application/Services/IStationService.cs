
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
    Task<IEnumerable<StationResponse>> GetAllAsync();
    List<string> getFieldNames();
    IEnumerable<StationResponse> SearchSelection(IEnumerable<StationResponse> stations, string searchString);
    Task UpdateAsync(StationModel stationModel);
    Task Delete(Guid Id);
}

