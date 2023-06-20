using AutoMapper;
using bikeRental.Application.Exceptions;
using bikeRental.Application.Models.Bicycle;
using bikeRental.Application.Models.Station;
using bikeRental.Core.Entities;
using bikeRental.DataAccess.Repositories;
using bikeRental.DataAccess.Repositories.Impl;
using Newtonsoft.Json;


namespace bikeRental.Application.Services.Impl;
public class StationService : IStationService
{
    private readonly IMapper _mapper;
    private readonly IStationRepository<Station> _stationRepository;
    public StationService(IStationRepository<Station> stationRepository, IMapper mapper)
    {
        _stationRepository = stationRepository;
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

    public IEnumerable<StationResponse> GetAll()
    {
        var response = _stationRepository.GetAll();
        return _mapper.Map<IEnumerable<StationResponse>>(response);
    }

    public string GetAddressess()
    {
        var response = _stationRepository.GetAll();
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

        var stations = _stationRepository.GetAll();

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


}

