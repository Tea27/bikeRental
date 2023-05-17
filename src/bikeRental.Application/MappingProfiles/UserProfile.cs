using AutoMapper;
using bikeRental.Application.Models.User;
using bikeRental.Core.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace bikeRental.Application.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserModel, ApplicationUser>();
        CreateMap<UserModel, ApplicationUser>();
        CreateMap<ApplicationUser, UserModel>();
        CreateMap<EditUserModel, UserModel>();
        CreateMap<UserModel, EditUserModel>();
        CreateMap<ApplicationUser, EditUserModel>();
        CreateMap<EditUserModel, ApplicationUser>();
        CreateMap<IdentityUser, ApplicationUser>();
      //  CreateMap<ApplicationUser, RegisterUserModel>();
        CreateMap<RegisterUserModel, ApplicationUser>();

    }
}
