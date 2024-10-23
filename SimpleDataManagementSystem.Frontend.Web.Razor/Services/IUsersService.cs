using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public interface IUsersService
    {
        Task<int> AddNewUserAsync(NewUserViewModel newUserViewModel);
        Task<UsersViewModel> GetAllUsersAsync(int? take = 8, int? page = 1);
        Task<UserViewModel> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(int userId, UpdateUserViewModel updateUserViewModel);
        Task DeleteUserAsync(int userId);
        Task UpdatePasswordAsync(int userId, UpdatePasswordViewModel updatePasswordViewModel);
    }
}
