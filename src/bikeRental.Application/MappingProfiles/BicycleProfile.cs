using bikeRental.Application.Models.Station;
using AutoMapper;
using bikeRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bikeRental.Application.Models.Bicycle;

namespace bikeRental.Application.MappingProfiles
{
    public class BicycleProfile : Profile
    {
        public BicycleProfile()
        {
            CreateMap<Bicycle, BicycleModel>();
            CreateMap<BicycleModel, Bicycle>();
        }
    }
}
