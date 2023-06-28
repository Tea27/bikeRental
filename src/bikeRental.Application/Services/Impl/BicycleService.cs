using bikeRental.Application.Models.Bicycle;
using bikeRental.Core.Entities;
using AutoMapper;
using bikeRental.DataAccess.Repositories;
using bikeRental.Core.Enums;
using bikeRental.Application.Models.Station;
using System.Data.Entity;
using bikeRental.Application.Exceptions;

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
   
    public async Task Delete(Guid id)
    {        
        var bicycle = await _bicycleRepository.GetByIdAsync(id) ?? throw new BadRequestException("Bicycle not found.");
        await _bicycleRepository.DeleteAsync(bicycle);
    }

    public async Task<BicycleModel> GetByIdAsync(Guid? id)
    {
        var response = await _bicycleRepository.GetByIdAsync(id) ?? throw new BadRequestException("Bicycle not found.");
        return _mapper.Map<BicycleModel>(response);
    }

    public IEnumerable<BicycleModel> CheckSwitch(string filterString, string searchString, string sortOrder, Guid? Id = null)
    {
        bool SearchIsEmpty = String.IsNullOrEmpty(searchString);
        bool FilterIsEmpty = String.IsNullOrEmpty(filterString);

        var bicycles = Id != null ? _bicycleRepository.FindByCondition(bicycle => bicycle.Station.Id == Id) : _bicycleRepository.GetAll();

        bicycles = (SearchIsEmpty, FilterIsEmpty) switch
        {
            (false, true) => Search(Sort(bicycles, sortOrder), searchString),
            (true, false) => Filter(Sort(bicycles, sortOrder), filterString),
            (false, false) => Search(Filter(Sort(bicycles, sortOrder), filterString), searchString),
            _ => Sort(bicycles, sortOrder),
        } ;

        return _mapper.Map<IEnumerable<BicycleModel>>(bicycles);
    }
    public IQueryable<Bicycle> Search(IQueryable<Bicycle> bicycles, string searchString)
    {
        return _bicycleRepository.FindByCondition(bicycles, bicycle => (bicycle.Description.ToLower().Contains(searchString.Trim().ToLower())
                                                            || bicycle.Station.Address.ToLower().Contains(searchString.Trim().ToLower())));
    }
    public IQueryable<Bicycle> Filter(IQueryable<Bicycle> bicycles, string filterString)
    {
        switch (filterString)
        {
            case "Acoustic":
                return _bicycleRepository.FindByCondition(bicycles, bicycle => bicycle.Type == BikeType.Acoustic);          
            default:
                return _bicycleRepository.FindByCondition(bicycles, bicycle => bicycle.Type == BikeType.Electric);
        }
    }

    public IQueryable<Bicycle> Sort(IQueryable<Bicycle> bicycles, string sortOrder)
    {
        switch (sortOrder)
        {
            case "StationDesc":
                return bicycles.OrderByDescending(b => b.Station.Address);
            case "Description":
                return bicycles.OrderBy(b => b.Description);
            case "DescriptionDesc":
                return bicycles.OrderByDescending(b => b.Description);
            default:
                return bicycles.OrderBy(b => b.Station.Address);
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
        await _bicycleRepository.UpdateAsync(bicycleNew);
    }

    public async Task<BicycleModel> GetByIdAsyncIncludeOrders(Guid? id)
    {
        var response = await _bicycleRepository.GetByIdAsyncIncludeOrders(id);
        return _mapper.Map<BicycleModel>(response);
    }





}

