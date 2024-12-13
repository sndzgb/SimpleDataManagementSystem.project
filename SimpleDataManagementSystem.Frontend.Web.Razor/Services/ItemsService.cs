using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using System.Net.Mime;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using SimpleDataManagementSystem.Frontend.Web.Razor.Extensions;
using System.Web;
using Microsoft.AspNetCore.Http;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public class ItemsService : IItemsService
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public ItemsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        

        public async Task<CreateItemResponseViewModel> CreateItemAsync(CreateItemRequestViewModel createItemRequestViewModel, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new MultipartFormDataContent();

            if (createItemRequestViewModel.URLdoslike != null)
            {
                var fileContent = new StreamContent(createItemRequestViewModel.URLdoslike.OpenReadStream());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(createItemRequestViewModel.URLdoslike.ContentType);

                content.Add(fileContent, "URLdoslike", createItemRequestViewModel.URLdoslike.FileName);
            }

            content.Add(
                new StringContent(
                    Convert.ToString(createItemRequestViewModel.IsEnabled), Encoding.UTF8, MediaTypeNames.Text.Plain
                ),
                "isEnabled"
            );
            content.Add
            (
                new StringContent
                (
                    createItemRequestViewModel.Nazivproizvoda ?? "", Encoding.UTF8, MediaTypeNames.Text.Plain
                ), 
                "nazivproizvoda"
            );
            content.Add(new StringContent(createItemRequestViewModel.Datumakcije ?? string.Empty, Encoding.UTF8, MediaTypeNames.Text.Plain), "datumakcije");
            content.Add(new StringContent(createItemRequestViewModel.Cijena.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "cijena");
            content.Add(new StringContent(Convert.ToString(createItemRequestViewModel.Kategorija), Encoding.UTF8, MediaTypeNames.Text.Plain), "kategorija");
            content.Add(new StringContent(Convert.ToString(createItemRequestViewModel.RetailerId), Encoding.UTF8, MediaTypeNames.Text.Plain), "retailerId");
            content.Add(new StringContent(createItemRequestViewModel.Opis ?? string.Empty, Encoding.UTF8, MediaTypeNames.Text.Plain), "opis");

            var response = await httpClient.PostAsync("api/items", content, cancellationToken);


            // TODO finish error handling!
            //var r = await response.HandleResponseAsync<string>();
            
            return await response.HandleResponseAsync<CreateItemResponseViewModel>(cancellationToken);

            //await response.HandleIfInvalidResponseAsync();

            //var j = await response.Content.ReadAsStringAsync();
            //return j;

            //if (!response.IsSuccessStatusCode)
            //{
            //    var json = await response.Content.ReadAsStringAsync();
            //    var message = JsonSerializer.Deserialize<ErrorViewModel>(json);
            //    throw new WebApiCallException(message);
            //}
            //else
            //{
            //    var j = await response.Content.ReadAsStringAsync();
            //    return j;
            //}
        }

        public async Task DeleteItemAsync(string itemId, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.DeleteAsync($"/api/items/{Uri.EscapeDataString(itemId)}", cancellationToken);

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

        public async Task<GetMultipleItemsResponseViewModel> GetMultipleItemsAsync(
                CancellationToken cancellationToken, 
                bool enabledOnly = true, 
                int? take = 8, 
                int? page = 1
            )
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync($"/api/items?take={take}&page={page}&enabled_only={enabledOnly}", cancellationToken);

            return await response.HandleResponseAsync<GetMultipleItemsResponseViewModel>(cancellationToken);


            //if (!response.IsSuccessStatusCode)
            //{
            //    using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            //    var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

            //    throw new WebApiCallException(message);
            //}
            //else
            //{
            //    var json = await response.Content.ReadAsStringAsync(cancellationToken);

            //    var responseContent = JsonSerializer.Deserialize<GetMultipleItemsResponseViewModel>(json);

            //    return responseContent;
            //}
        }

        public async Task<GetSingleItemResponseViewModel> GetSingleItemAsync(string itemId, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync($"/api/items/{Uri.EscapeDataString(itemId)}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var error = JsonSerializer.Deserialize<ErrorViewModel>(json);

                throw new WebApiCallException(error);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var responseContent = JsonSerializer.Deserialize<GetSingleItemResponseViewModel>(json);
                return responseContent;
            }
        }

        public async Task UpdateItemAsync(string itemId, UpdateItemViewModel updateItemViewModel, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new MultipartFormDataContent();

            if (updateItemViewModel.URLdoslike != null)
            {
                var fileContent = new StreamContent(updateItemViewModel.URLdoslike.OpenReadStream());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(updateItemViewModel.URLdoslike.ContentType);

                content.Add(fileContent, "URLdoslike", updateItemViewModel.URLdoslike.FileName);
            }

            content.Add
            (
                new StringContent
                (
                    Convert.ToString(updateItemViewModel.IsEnabled), 
                    Encoding.UTF8, 
                    MediaTypeNames.Text.Plain
                ), 
                "isEnabled"
            );
            content.Add
            (
                new StringContent
                (
                    Convert.ToString(updateItemViewModel.IsMonitoredByUser), 
                    Encoding.UTF8, 
                    MediaTypeNames.Text.Plain
                ),
                "isMonitoredByUser"
            );
            content.Add
            (
                new StringContent
                (
                    string.IsNullOrEmpty(updateItemViewModel.Datumakcije) ? string.Empty : updateItemViewModel.Datumakcije, 
                    Encoding.UTF8, 
                    MediaTypeNames.Text.Plain
                ), "datumakcije"
            );
            content.Add(new StringContent(updateItemViewModel.Cijena.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "cijena");
            content.Add
            (
                new StringContent
                (
                    Convert.ToString(updateItemViewModel.Kategorija), Encoding.UTF8, MediaTypeNames.Text.Plain
                ), 
                "kategorija"
            );
            content.Add
            (
                new StringContent
                (
                    Convert.ToString(updateItemViewModel.RetailerId), Encoding.UTF8, MediaTypeNames.Text.Plain
                ), 
                "retailerId"
            );
            content.Add
            (
                new StringContent
                (
                    string.IsNullOrWhiteSpace(updateItemViewModel.Opis) ? string.Empty : updateItemViewModel.Opis, Encoding.UTF8, MediaTypeNames.Text.Plain
                ), 
                "opis"
            );
            content.Add
            (
                new StringContent
                (
                    updateItemViewModel.DeleteCurrentURLdoslike.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain
                ),
                "deleteCurrentURLdoslike"
            );

            var response = await httpClient.PutAsync($"api/items/{Uri.EscapeDataString(itemId)}", content, cancellationToken);

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

        public async Task<SearchItemsResponseViewModel> SearchItemsAsync(
                SearchItemsRequestViewModel itemsSearchRequestViewModel, 
                CancellationToken cancellationToken
            )
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var properties = from p in itemsSearchRequestViewModel.GetType().GetProperties()
                             where p.GetValue(itemsSearchRequestViewModel, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(itemsSearchRequestViewModel, null)?.ToString());

            string queryString = String.Join("&", properties.ToArray());

            var response = await httpClient.GetAsync($"/api/items/search" + $"?{queryString}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var responseContent = JsonSerializer.Deserialize<SearchItemsResponseViewModel>(json);
                return responseContent!;
            }
        }

        public async Task ToggleItemEnabledDisabledStatusAsync(string itemId, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            //var content = new StringContent(JsonSerializer.Serialize(itemId), Encoding.UTF8, "application/json");

            //var response = await httpClient.PostAsync("api/items/status", content, cancellationToken);
            var response = await httpClient.PostAsync($"api/items/{Uri.EscapeDataString(itemId)}/status", null, cancellationToken);

            await response.HandleResponseAsync<object>(cancellationToken);

            //if (!response.IsSuccessStatusCode)
            //{
            //    using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            //    var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

            //    throw new WebApiCallException(message);
            //}
            //else
            //{
            //    return;
            //}
        }

        public async Task ToggleMonitoredItemAsync(string itemId, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            //var json = JsonSerializer.Serialize(new { itemId = itemId });

            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"api/items/{Uri.EscapeDataString(itemId)}/monitored", null, cancellationToken);

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
