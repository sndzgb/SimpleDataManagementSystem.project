using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public interface IRolesService
    {
		Task<GetAllRolesResponseViewModel> GetAllRolesAsync(CancellationToken cancellationToken);
    }
}
