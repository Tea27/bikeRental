using bikeRental.Application.Models.Station;
using AutoMapper;
using bikeRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace bikeRental.Application.MappingProfiles
{
    public class StationProfile : Profile
    {
        public StationProfile() {
            CreateMap<Station, StationResponse>();
            CreateMap<Station, StationModel>();
            CreateMap<StationModel, Station>();
        }
    }
}
