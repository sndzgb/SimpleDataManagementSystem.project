using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using System.Text.Json;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        //public static async Task HandleIfInvalidResponseAsync(this HttpResponseMessage httpResponseMessage) 
        //{
        //    if (httpResponseMessage.IsSuccessStatusCode)
        //    {
        //        return;
        //    }

        //    if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //    {
        //        throw new UnauthorizedException("You must be logged in to view this resource");
        //    }

        //    if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
        //    {
        //        throw new ForbiddenException("You are not allowed to access this resource");
        //    }

        //    using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
        //    var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);
        //    throw new WebApiCallException(message);
        //}

        public static async Task<T?> HandleResponseAsync<T>(this HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var json = await httpResponseMessage.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(json))
                {
                    return default(T?);
                }

                var model = JsonSerializer.Deserialize<T>(json);

                return await Task.FromResult(model);
            }

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException("You must be logged in to view this resource");
            }

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new ForbiddenException("You are not allowed to access this resource");
            }

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);
            throw new WebApiCallException(message);
        } 
    }
}
