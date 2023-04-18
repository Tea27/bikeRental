using AutoMapper;
using bikeRental.Application.Models.User;
using bikeRental.Core.Identity;

namespace bikeRental.Application.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserModel, ApplicationUser>();
    }
}
