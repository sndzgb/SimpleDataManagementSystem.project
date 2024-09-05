using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using System.Text.Json;
using System.Text;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public CategoriesService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<int> AddNewCategoryAsync(NewCategoryViewModel newCategoryViewModel)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new StringContent(JsonSerializer.Serialize(newCategoryViewModel), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/categories", content);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var responseContent = await JsonSerializer.DeserializeAsync<int>(contentStream);

                return Convert.ToInt32(responseContent);
            }
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.DeleteAsync($"api/categories/{categoryId}");

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        public async Task<CategoriesViewModel> GetAllCategoriesAsync(int? take = 8, int? page = 1)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync($"/api/categories?take={take}&page={page}");

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync();
                var responseContent = JsonSerializer.Deserialize<CategoriesViewModel>(json);
                return responseContent;
            }
        }

        public async Task<CategoryViewModel> GetCategoryByIdAsync(int categoryId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync($"/api/categories/{categoryId}");

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync();
                var responseContent = JsonSerializer.Deserialize<CategoryViewModel>(json);
                return responseContent;
            }
        }

        public async Task UpdateCategoryAsync(int categoryId, UpdateCategoryViewModel updateCategoryViewModel)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new StringContent(JsonSerializer.Serialize(updateCategoryViewModel), Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"api/categories/{categoryId}", content);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

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
