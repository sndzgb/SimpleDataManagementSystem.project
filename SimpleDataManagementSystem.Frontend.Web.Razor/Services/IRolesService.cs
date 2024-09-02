using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public interface IRolesService
    {
        Task<List<RoleViewModel>> GetAllRolesAsync();
    }
}
