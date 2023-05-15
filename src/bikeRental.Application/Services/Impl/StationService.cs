using AutoMapper;
using bikeRental.Application.Models.Station;
using bikeRental.Core.Entities;
using bikeRental.DataAccess.Repositories;
using bikeRental.DataAccess.Repositories.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        station =  await _stationRepository.AddAsync(station);
        return _mapper.Map<StationModel>(station);
    }

    public async Task<StationModel> GetByIdAsync(Guid? id)
    {
        var response = await _stationRepository.GetByIdAsync(id);
        return _mapper.Map<StationModel>(response);
    }

    public async Task<IEnumerable<StationResponse>> GetAllAsync()
    {
        var response = await _stationRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<StationResponse>>(response);
    }

    public List<string> getFieldNames()
    {
        Station Station = new Station();
        return Station.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
    }

    public IEnumerable<StationResponse> SearchSelection(IEnumerable<StationResponse> stations, string searchString)
    {
        IEnumerable<StationResponse> stationsSearched = stations.ToList();

        if (!String.IsNullOrEmpty(searchString))
        {
            var searchStrTrim = searchString.Trim();
            stationsSearched = stations.Where(t => t.Address.Contains(searchStrTrim));
        }
        return stationsSearched;
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
}

