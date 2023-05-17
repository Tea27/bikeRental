using bikeRental.Application.Models;
using bikeRental.Application.Models.Station;
using bikeRental.Application.Models.User;

namespace bikeRental.Application.Services;

public interface IUserService
{
    Task<BaseResponseModel> ChangePasswordAsync(Guid userId, ChangePasswordModel changePasswordModel);
    Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel);
    Task<CreateUserResponseModel> CreateAsync(CreateUserModel createUserModel);
    Task<LoginResponseModel> LoginAsync(LoginUserModel loginUserModel);
    Task<IEnumerable<UserModel>> GetAllUsers();
    Task<UserModel> GetByIdAsync(Guid? id);
    Task DeleteAsync(Guid Id);
    Task UpdateAsync(EditUserModel userModel);
    Task AddAsync(RegisterUserModel userModel);
}
