using bikeRental.Application.Models;
using bikeRental.Application.Models.Station;
using bikeRental.Application.Models.User;
using Microsoft.AspNetCore.Identity;

namespace bikeRental.Application.Services;

public interface IUserService
{
    Task<BaseResponseModel> ChangePasswordAsync(Guid userId, ChangePasswordModel changePasswordModel);
    Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel);
    Task<CreateUserResponseModel> CreateAsync(CreateUserModel createUserModel);
    Task<IEnumerable<UserModel>> GetAllUsers();
    Task<UserModel> GetByIdAsync(Guid? id);
    Task DeleteAsync(Guid Id);
    Task UpdateAsync(EditUserModel userModel);
    Task AddAsync(RegisterUserModel userModel);
    Task<SignInResult> LoginAsync(LoginUserModel userModel);
    Task LogoutAsync();
    IEnumerable<UserModel> SearchSelection(IEnumerable<UserModel> users, string searchString);
    IEnumerable<UserModel> SortingSelection(IEnumerable<UserModel> users, string sortOrder);
}
