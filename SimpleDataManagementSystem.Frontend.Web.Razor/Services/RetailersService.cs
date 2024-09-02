using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public class RetailersService : IRetailersService
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public RetailersService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<int> AddNewRetailerAsync(NewRetailerViewModel newRetailerViewModel)
        {
            var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

            var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(newRetailerViewModel.LogoImage.OpenReadStream());
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(newRetailerViewModel.LogoImage.ContentType);

            content.Add(fileContent, "LogoImage", newRetailerViewModel.LogoImage.FileName);

            //var jsonPayload = JsonSerializer.Serialize(newRetailerViewModel);
            //var jsonBytes = Encoding.UTF8.GetBytes(jsonPayload);
            //var jsonContent = new StreamContent(new MemoryStream(jsonBytes));
            //jsonContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            content.Add(new StringContent(newRetailerViewModel.Name, Encoding.UTF8, MediaTypeNames.Text.Plain), "name");
            content.Add(new StringContent(Convert.ToString(newRetailerViewModel.Priority), Encoding.UTF8, MediaTypeNames.Text.Plain), "priority");

            //content.Add(new StringContent(JsonSerializer.Serialize(newRetailerViewModel), Encoding.UTF8, "application/json"), "Json");

            //content.Add(jsonContent, "payload", "metadata.json");
            //content.Add(jsonContent);

            var response = await httpClient.PostAsync("api/retailers", content);

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

        public async Task DeleteRetailerAsync(int retailerId)
        {
            var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

            var response = await httpClient.DeleteAsync($"/api/retailers/{retailerId}");

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                return;
            }
        }

        public async Task<List<RetailerViewModel>> GetAllRetailersAsync(int? take = 8, int? page = 1)
        {
            var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

            var response = await httpClient.GetAsync($"/api/retailers?take={take}&page={page}");

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync();
                var responseContent = JsonSerializer.Deserialize<List<RetailerViewModel>>(json);
                return responseContent;
            }
        }

        public async Task<RetailerViewModel> GetRetailerByIdAsync(int retailerId)
        {
            var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

            var response = await httpClient.GetAsync($"/api/retailers/{retailerId}");

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync();
                var responseContent = JsonSerializer.Deserialize<RetailerViewModel>(json);
                return responseContent;
            }
        }

        public async Task UpdateRetailerAsync(int retailerId, UpdateRetailerViewModel updateRetailerViewModel)
        {
            var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

            var content = new MultipartFormDataContent();

            if (updateRetailerViewModel.LogoImage != null)
            {
                var fileContent = new StreamContent(updateRetailerViewModel.LogoImage.OpenReadStream());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(updateRetailerViewModel.LogoImage.ContentType);

                content.Add(fileContent, "LogoImage", updateRetailerViewModel.LogoImage.FileName);
            }

            content.Add(new StringContent(updateRetailerViewModel.Name, Encoding.UTF8, MediaTypeNames.Text.Plain), "name");
            content.Add(new StringContent(Convert.ToString(updateRetailerViewModel.Priority), Encoding.UTF8, MediaTypeNames.Text.Plain), "priority");

            var response = await httpClient.PutAsync($"api/retailers/{retailerId}", content);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                return;
            }
        }
    }
}
