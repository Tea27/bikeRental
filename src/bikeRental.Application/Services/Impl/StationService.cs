using AutoMapper;
using bikeRental.Application.Exceptions;
using bikeRental.Application.Models.Station;
using bikeRental.Application.Models.User;
using bikeRental.Core.Entities;
using bikeRental.DataAccess.Repositories;
using bikeRental.DataAccess.Repositories.Impl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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

    public IEnumerable<StationResponse> SearchSelection(string searchString)
    {
        var stations = _stationRepository.FindByCondition(station => station.Address.ToLower().Contains(searchString.Trim().ToLower()));
        return _mapper.Map<IEnumerable<StationResponse>>(stations);
    }
    public IEnumerable<StationResponse> SortingSelection(string sortOrder)
    {
        var stations = _stationRepository.GetAll();
        switch (sortOrder)
        {
            case "AddressDesc":
                stations = stations.OrderByDescending(s => s.Address);
                break;
            default:
                stations = stations.OrderBy(s => s.Address);
                break;
        }
        return _mapper.Map<IEnumerable<StationResponse>>(stations);
    }


}

