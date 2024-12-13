using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using System.Text.Json;
using System.Text;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public CategoriesService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task CreateCategoryAsync(CreateCategoryViewModel createCategoryViewModel, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new StringContent(JsonSerializer.Serialize(createCategoryViewModel), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/categories", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                return;
                //using var contentStream = await response.Content.ReadAsStreamAsync();

                //var responseContent = await JsonSerializer.DeserializeAsync<int>(contentStream);

                //return Convert.ToInt32(responseContent);
            }
        }

        public async Task DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.DeleteAsync($"api/categories/{categoryId}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                return;
            }
        }

        public async Task<GetMultipleCategoriesResponseViewModel> GetMultipleCategoriesAsync(
                CancellationToken cancellationToken, int? take = 8, int? page = 1
            )
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync($"/api/categories?take={take}&page={page}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var responseContent = JsonSerializer.Deserialize<GetMultipleCategoriesResponseViewModel>(json);
                return responseContent;
            }
        }

        public async Task<GetSingleCategoryResponseViewModel> GetSingleCategoryAsync(int categoryId, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync($"/api/categories/{categoryId}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var responseContent = JsonSerializer.Deserialize<GetSingleCategoryResponseViewModel>(json);
                return responseContent;
            }
        }

        public async Task UpdateCategoryAsync(
                int categoryId, 
                UpdateCategoryViewModel updateCategoryViewModel, 
                CancellationToken cancellationToken
            )
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new StringContent(JsonSerializer.Serialize(updateCategoryViewModel), Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"api/categories/{categoryId}", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                await Task.CompletedTask;
            }
        }
    }
}
