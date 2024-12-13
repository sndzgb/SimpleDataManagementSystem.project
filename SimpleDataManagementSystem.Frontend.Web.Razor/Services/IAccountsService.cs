using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public interface IAccountsService
    {
        Task<LogInResponseViewModel?> LogInAsync(string username, string password, CancellationToken cancellationToken);
        Task<GetSingleUserResponseViewModel?> GetAccountDetailsAsync(CancellationToken cancellationToken);
        Task UpdatePasswordAsync(string oldPassword, string newPassword, CancellationToken cancellationToken);
    }
}
