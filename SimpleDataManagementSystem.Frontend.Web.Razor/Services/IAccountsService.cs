using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public interface IAccountsService
    {
        Task<string?> LogInAsync(string username, string password);
    }
}
