using bikeRental.Application.Models;
using bikeRental.Application.Models.Station;
using bikeRental.Application.Models.User;
using bikeRental.Core.Identity;
using Microsoft.AspNetCore.Identity;

namespace bikeRental.Application.Services;

public interface IUserService
{
    Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel);
    Task<CreateUserResponseModel> CreateAsync(CreateUserModel createUserModel);
    IEnumerable<UserModel> GetAllUsers(IQueryable<ApplicationUser> response);
    Task<UserModel> GetByIdAsync(Guid? id);
    Task DeleteAsync(Guid Id);
    Task UpdateAsync(EditUserModel userModel);
    Task AddAsync(RegisterUserModel userModel);
    Task<SignInResult> LoginAsync(LoginUserModel userModel);
    Task LogoutAsync();
    //IEnumerable<UserModel> SearchSelection(IEnumerable<UserModel> users, string searchString);
    //IEnumerable<UserModel> SortingSelection(IEnumerable<UserModel> users, string sortOrder);
    Task<UserModel> GetByIdAsyncIncludeOrders(Guid? id);
    IEnumerable<UserModel> CheckSwitch(string filterString, string searchString, string sortOrder);
}
