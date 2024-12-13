using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public interface IUsersService
    {
        Task<int> CreateUserAsync(CreateUserViewModel createUserViewModel, CancellationToken cancellationToken);
        Task<GetMultipleUsersResponseViewModel> GetMultipleUsersAsync(CancellationToken cancellationToken, int? take = 8, int? page = 1);
        Task<GetSingleUserResponseViewModel> GetSingleUserAsync(int userId, CancellationToken cancellationToken);
        Task UpdateUserAsync(int userId, UpdateUserViewModel updateUserViewModel, CancellationToken cancellationToken);
        Task DeleteUserAsync(int userId, CancellationToken cancellationToken);
    }
}
