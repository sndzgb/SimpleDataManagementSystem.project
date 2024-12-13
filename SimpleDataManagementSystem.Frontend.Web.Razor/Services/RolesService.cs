using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using System.Text.Json;
using System.Text;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public class RolesService : IRolesService
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public RolesService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


		public async Task<GetAllRolesResponseViewModel> GetAllRolesAsync(CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync("api/roles", cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var responseContent = JsonSerializer.Deserialize<GetAllRolesResponseViewModel>(json);
                return responseContent;
            }
        }
    }
}
