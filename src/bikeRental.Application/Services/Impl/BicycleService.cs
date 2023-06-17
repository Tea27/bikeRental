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
using bikeRental.DataAccess.Repositories.Impl;
using static System.Collections.Specialized.BitVector32;
using bikeRental.Core.Enums;

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
    public async Task<IEnumerable<BicycleModel>> GetAllAsync()
    {
        var bicycles = _bicycleRepository.GetAll();

        foreach (var bicycle in bicycles)
        {
            var station = await _stationService.GetByIdAsync(bicycle.Station.Id);

            bicycle.Station = _mapper.Map<Station>(station);
        }
        var bicyclesModels = _mapper.Map<IEnumerable<BicycleModel>>(bicycles);
        return bicyclesModels;

    }

    public async Task Delete(Guid Id, Guid stationId)
    {
        var bicycle = await _bicycleRepository.GetByIdAsync(Id);
        await _bicycleRepository.DeleteAsync(Id);
    }

    public async Task<IEnumerable<BicycleModel>> GetByStation(Guid StationId)
    {
        var bicycles = await _bicycleRepository.GetByStation(StationId);
        var bicyclesModel = _mapper.Map<IEnumerable<BicycleModel>>(bicycles);
        foreach (var bicycle in bicyclesModel)
        {
            bicycle.Station = await _stationService.GetByIdAsync(StationId);
        }
        return bicyclesModel;
    }

    public async Task<BicycleModel> GetByIdAsync(Guid? id)
    {
        var response = await _bicycleRepository.GetByIdAsync(id);
        return _mapper.Map<BicycleModel>(response);
    }

    public IEnumerable<BicycleModel> SearchSelection(Guid Id, string searchString)
    {
        var bicycles = _bicycleRepository.FindByCondition(bicycle => bicycle.Description.ToLower().Contains(searchString.Trim().ToLower()) && bicycle.Station.Id == Id);
        return _mapper.Map<IEnumerable<BicycleModel>>(bicycles);
    }
    public IEnumerable<BicycleModel> SearchSelection(string searchString)
    {
        var bicycles = _bicycleRepository.FindByCondition(bicycle => bicycle.Description.ToLower().Contains(searchString.Trim().ToLower()));
        return _mapper.Map<IEnumerable<BicycleModel>>(bicycles);
    }
    public IEnumerable<BicycleModel> FilterSelection(Guid Id, string filterString)
    {
        var bicycles = FilterSwitch(Id, filterString);
        return _mapper.Map<IEnumerable<BicycleModel>>(bicycles);
    }
    public IEnumerable<BicycleModel> FilterSelection(string filterString)
    {
        var bicycles = FilterSwitch(filterString);
        return _mapper.Map<IEnumerable<BicycleModel>>(bicycles);
    }

    public IQueryable<Bicycle> FilterSwitch(string filterString)
    {
        switch (filterString)
        {
            case "Acoustic":
                return _bicycleRepository.FindByCondition(bicycle => bicycle.Type == BikeType.Acoustic);
            case "Electric":
                return _bicycleRepository.FindByCondition(bicycle => bicycle.Type == BikeType.Electric);
            default:
                return _bicycleRepository.GetAll();
        }
    }
    public IQueryable<Bicycle> FilterSwitch(Guid Id, string filterString)
    {
        switch (filterString)
        {
            case "Acoustic":
                return _bicycleRepository.FindByCondition(bicycle => bicycle.Type == BikeType.Acoustic && bicycle.Station.Id == Id);
            case "Electric":
                return _bicycleRepository.FindByCondition(bicycle => bicycle.Type == BikeType.Electric && bicycle.Station.Id == Id);
            default:
                return _bicycleRepository.FindByCondition(bicycle => bicycle.Station.Id == Id);
        }
    }

    public async Task<BicycleModel> AddAsync(BicycleModel bicycleModel, Guid stationId)
    {
        var bicycle = _mapper.Map<Bicycle>(bicycleModel);

        bicycle = await _bicycleRepository.AddAsync(bicycle, stationId);

        return _mapper.Map<BicycleModel>(bicycle);
    }

    public async Task UpdateAsync(BicycleModel bicycleModel)
    {
        var bicycleNew = _mapper.Map<Bicycle>(bicycleModel);
        var bicycleOld = await _bicycleRepository.GetByIdAsync(bicycleModel.Id);

        bicycleModel.Station = await _stationService.GetByIdAsync(bicycleModel.Station.Id);

        if (bicycleModel.Type.ToString() == "Electric" && bicycleOld.Type.ToString() != "Electric")
        {
            bicycleModel.Station.NumberOfBikes--;
            bicycleModel.Station.NumberOfElectricBikes++;
        }
        else if (bicycleModel.Type.ToString() == "Acoustic" && bicycleOld.Type.ToString() != "Acoustic")
        {
            bicycleModel.Station.NumberOfElectricBikes--;
            bicycleModel.Station.NumberOfBikes++;
        }

        await _stationService.UpdateAsync(bicycleModel.Station);
        await _bicycleRepository.UpdateAsync(bicycleNew, bicycleModel.Station.Id);
    }


}

