using AutoMapper;
using bikeRental.Application.Exceptions;
using bikeRental.Application.Models.Bicycle;
using bikeRental.Application.Models.Station;
using bikeRental.Core.Entities;
using bikeRental.Core.Enums;
using bikeRental.DataAccess.Repositories;
using bikeRental.DataAccess.Repositories.Impl;
using Newtonsoft.Json;


namespace bikeRental.Application.Services.Impl;
public class StationService : IStationService
{
    private readonly IMapper _mapper;
    private readonly IStationRepository<Station> _stationRepository;
    private readonly IBicycleRepository<Bicycle> _bicycleRepository;

    public StationService(IStationRepository<Station> stationRepository, IBicycleRepository<Bicycle> bicycleRepository, IMapper mapper)
    {
        _stationRepository = stationRepository;
        _bicycleRepository = bicycleRepository;
        _mapper = mapper;
    }

    public async Task<StationModel> AddAsync(StationModel stationModel)
    {
        var station = _mapper.Map<Station>(stationModel);
        station = await _stationRepository.AddAsync(station);
        return _mapper.Map<StationModel>(station);
    }

    public async Task<StationModel> GetByIdAsync(Guid? id)
    {
        var response = await _stationRepository.GetByIdAsync(id) ?? throw new BadRequestException("Station not found.");
        return _mapper.Map<StationModel>(response);
    }

    public string SaveError(Guid? id)
    {
        var bicyclesWithOrders = _bicycleRepository.FindByCondition(bicycle => bicycle.Station.Id == id && bicycle.Orders.Any());

        return bicyclesWithOrders.Any() ? "Station cannot be removed, bicycles have order relation: " + string.Join(", ", bicyclesWithOrders.Select(bicycle => bicycle.Description)) :
                                            "DB update exception please contact administrator";
    }

    public bool HasOrders(Guid? id)
    {
        return _bicycleRepository.FindByCondition(bicycle => bicycle.Station.Id == id && bicycle.Orders.Any()).Any();
    }

    public IEnumerable<StationResponse> GetAll()
    {    
        var response = _stationRepository.FindByCondition(station => station.Bicycles.Count == 0 || station.Bicycles.Any(b => b.Status != BikeStatus.Disabled));
        return _mapper.Map<IEnumerable<StationResponse>>(response);
    }

    public string GetAddressess()
    {
        var response = _stationRepository.FindByCondition(station => station.Bicycles.Count == 0 || station.Bicycles.Any(b => b.Status != BikeStatus.Disabled));
        var addresses = response.Select(station => new
        {
            title = station.Address,
            lat = station.lattitude,
            lng = station.longitude,
            description = station.Address
        });
        var json = JsonConvert.SerializeObject(addresses);
        return json;
    }


    public async Task UpdateAsync(StationModel stationModel)
    {
        var station = _mapper.Map<Station>(stationModel);
        await _stationRepository.UpdateAsync(station);
    }
    public async Task Delete(Guid Id)
    {
        await _stationRepository.DeleteAsync(Id);

    }

    public IEnumerable<StationResponse> CheckSwitch(string searchString, string sortOrder)
    {
        bool SearchIsEmpty = String.IsNullOrEmpty(searchString);
        var stations = _stationRepository.FindByCondition(station => station.Bicycles.Count==0 || station.Bicycles.Any(b => b.Status != BikeStatus.Disabled));
        stations = (SearchIsEmpty) switch
        {
            false => Search(Sort(stations, sortOrder), searchString),
            _ => Sort(stations, sortOrder),
        };

        return _mapper.Map<IEnumerable<StationResponse>>(stations);
    }

    public IQueryable<Station> Search(IQueryable<Station> stations, string searchString)
    {
        return _stationRepository.FindByCondition(stations, station => station.Address.ToLower().Contains(searchString.Trim().ToLower()));
    }
    public IQueryable<Station> Sort(IQueryable<Station> stations, string sortOrder)
    {
        switch (sortOrder)
        {
            case "AddressDesc":
                return stations.OrderByDescending(s => s.Address);
            default:
                return stations.OrderBy(s => s.Address);
        }
    }

    public async Task DisableBicycles(Guid id)
    {
        
        var station =  await _stationRepository.GetByIdAsync(id) ?? throw new BadRequestException("Station not found.");
        var bicycles = station.Bicycles;
        foreach (var bicycle in bicycles)
        {
            if (bicycle.Orders.Count!=0)
            {
                bicycle.Status = BikeStatus.Disabled;
                await _bicycleRepository.UpdateAsync(bicycle);
            }
            else
            {
                await _bicycleRepository.DeleteAsync(bicycle);
            }
        }
    }


}

