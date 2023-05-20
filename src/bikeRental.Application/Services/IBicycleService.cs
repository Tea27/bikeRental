using bikeRental.Application.Models.Bicycle;
using bikeRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Application.Services;
public interface IBicycleService
{
    Task<IEnumerable<Bicycle>> GetAllAsync();
    Task<IEnumerable<BicycleModel>> GetByStation(Guid? StationId);

    List<string> getFieldNames();
    IEnumerable<BicycleModel> SearchSelection(IEnumerable<BicycleModel> bicycles, string searchString);
}

