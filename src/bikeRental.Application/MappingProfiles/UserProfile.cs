using AutoMapper;
using bikeRental.Application.Models.Order;
using bikeRental.Application.Models.User;
using bikeRental.Core.Entities;
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
            //.ForMember(dst => dst.UserName, opt => opt.MapFrom(model => model.UserName));
        CreateMap<EditUserModel, ApplicationUser>();
        CreateMap<RegisterUserModel, ApplicationUser>();
    }
}
