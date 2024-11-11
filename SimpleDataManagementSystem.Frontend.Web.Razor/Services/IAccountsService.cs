using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public interface IAccountsService
    {
        Task<AuthTokenViewModel?> LogInAsync(string username, string password);
        Task<UserViewModel?> GetAccountDetailsAsync();
        Task UpdatePasswordAsync(string oldPassword, string newPassword);
    }
}
