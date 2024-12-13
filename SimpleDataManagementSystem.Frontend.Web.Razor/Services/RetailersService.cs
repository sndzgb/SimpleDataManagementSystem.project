using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;
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


        public async Task<int> CreateRetailerAsync(CreateRetailerViewModel createRetailerViewModel, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new MultipartFormDataContent();

            if (createRetailerViewModel.LogoImage != null)
            {
                var fileContent = new StreamContent(createRetailerViewModel.LogoImage.OpenReadStream());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(createRetailerViewModel.LogoImage.ContentType);

                content.Add(fileContent, "LogoImage", createRetailerViewModel.LogoImage.FileName);
            }

            content.Add(new StringContent(createRetailerViewModel.Name, Encoding.UTF8, MediaTypeNames.Text.Plain), "name");
            content.Add(new StringContent(Convert.ToString(createRetailerViewModel.Priority), Encoding.UTF8, MediaTypeNames.Text.Plain), "priority");

            var response = await httpClient.PostAsync("api/retailers", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var responseContent = await JsonSerializer.DeserializeAsync<int>(contentStream);

                return Convert.ToInt32(responseContent);
            }
        }

        public async Task DeleteRetailerAsync(int retailerId, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.DeleteAsync($"/api/retailers/{retailerId}", cancellationToken);

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

        public async Task<GetMultipleRetailersResponseViewModel> GetMultipleRetailersAsync(
                CancellationToken cancellationToken, int? take = 8, int? page = 1
            )
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync($"/api/retailers?take={take}&page={page}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var responseContent = JsonSerializer.Deserialize<GetMultipleRetailersResponseViewModel>(json);
                return responseContent;
            }
        }

        public async Task<GetSingleRetailerResponseViewModel> GetSingleRetailerAsync(int retailerId, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync($"/api/retailers/{retailerId}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var responseContent = JsonSerializer.Deserialize<GetSingleRetailerResponseViewModel>(json);
                return responseContent;
            }
        }

        public async Task UpdateRetailerAsync(int retailerId, UpdateRetailerViewModel updateRetailerViewModel, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new MultipartFormDataContent();

            if (updateRetailerViewModel.LogoImage != null)
            {
                var fileContent = new StreamContent(updateRetailerViewModel.LogoImage.OpenReadStream());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(updateRetailerViewModel.LogoImage.ContentType);

                content.Add(fileContent, "LogoImage", updateRetailerViewModel.LogoImage.FileName);
            }

            content.Add(new StringContent(updateRetailerViewModel.Name, Encoding.UTF8, MediaTypeNames.Text.Plain), "name");
            content.Add(new StringContent(Convert.ToString(updateRetailerViewModel.Priority), Encoding.UTF8, MediaTypeNames.Text.Plain), "priority");
            content.Add
            (
                new StringContent
                (
                    updateRetailerViewModel.DeleteCurrentLogoImage.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain
                ),
                "deleteCurrentLogoImage"
            );

            var response = await httpClient.PutAsync($"api/retailers/{retailerId}", content, cancellationToken);

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
    }
}
