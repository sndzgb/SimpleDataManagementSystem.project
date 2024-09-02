using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using System.Text.Json;
using System.Text;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public class RolesService : IRolesService
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public RolesService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<List<RoleViewModel>> GetAllRolesAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

            var response = await httpClient.GetAsync("api/roles");
            
            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync();
                var responseContent = JsonSerializer.Deserialize<List<RoleViewModel>>(json);
                return responseContent;
            }
        }
    }
}
