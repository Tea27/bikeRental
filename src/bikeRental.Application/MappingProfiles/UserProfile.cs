using AutoMapper;
using bikeRental.Application.Models.User;
using bikeRental.DataAccess.Identity;

namespace bikeRental.Application.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserModel, ApplicationUser>();
    }
}
