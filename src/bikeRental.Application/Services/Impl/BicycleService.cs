using AutoMapper;
using bikeRental.Core.Entities;
using bikeRental.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Application.Services.Impl;
public class BicycleService : IBicycleService
{
    private readonly IMapper _mapper;
    private readonly IBicycleRepository<Bicycle> _bicycleRepository;
    public BicycleService(IBicycleRepository<Bicycle> bicycleRepository, IMapper mapper)
    {
        _bicycleRepository = bicycleRepository;
        _mapper = mapper;
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

}

