using AutoMapper;
using bikeRental.Core.Entities;
using bikeRental.Application.Models.Bicycle;
using bikeRental.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bikeRental.Application.Models.Station;

namespace bikeRental.Application.Services.Impl;
public class BicycleService : IBicycleService
{
    private readonly IMapper _mapper;
    private readonly IBicycleRepository<Bicycle> _bicycleRepository;
    private readonly IStationService _stationService;
    public BicycleService(IBicycleRepository<Bicycle> bicycleRepository, IMapper mapper, IStationService stationService)
    {
        _bicycleRepository = bicycleRepository;
        _mapper = mapper;
        _stationService = stationService;
    }
    public async Task<IEnumerable<Bicycle>> GetAllAsync()
    {
        return await _bicycleRepository.GetAllAsync();

    }

    public List<string> getFieldNames()
    {
        Bicycle Bicycle = new Bicycle();
        return Bicycle.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
    }

    public async Task<IEnumerable<BicycleModel>> GetByStation(Guid? StationId)
    {
        var bicycles = await _bicycleRepository.GetByStation(StationId);
        var bicyclesModel = _mapper.Map<IEnumerable<BicycleModel>>(bicycles);
        foreach (var bicycle in bicyclesModel)
        {
            bicycle.Station = await _stationService.GetByIdAsync(StationId);
        }
        return bicyclesModel;
    }

    public IEnumerable<BicycleModel> SearchSelection(IEnumerable<BicycleModel> bicycles, string searchString)
    {
        IEnumerable<BicycleModel> stationsSearched = bicycles.ToList();

        if (!String.IsNullOrEmpty(searchString))
        {
            var searchStrTrim = searchString.Trim();
            stationsSearched = bicycles.Where(t => t.Description.Contains(searchStrTrim));
        }
        return stationsSearched;
    }


}

