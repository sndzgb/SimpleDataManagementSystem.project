using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Records;

namespace SimpleDataManagementSystem.Backend.WebAPI.Services.Abstractions
{
    public interface ITokenGeneratorService
    {
        /// <summary>
        /// Generates authentication token based on provided authenticatd user parameter.
        /// </summary>
        /// <param name="authenticatedUser"></param>
        /// <returns></returns>
        Task<string?> GenerateTokenAsync(AuthenticatedUser authenticatedUser);
    }
}
