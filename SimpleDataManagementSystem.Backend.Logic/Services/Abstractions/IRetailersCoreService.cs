using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Exceptions;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Abstractions
{
    public interface IRetailersCoreService
    {
        Task<GetRetailerResponseDTO?> GetRetailerAsync(GetRetailerRequestDTO getRetailerRequestDTO, CancellationToken cancellationToken);
        Task<GetRetailersResponseDTO?> GetRetailersAsync(GetRetailersRequestDTO getRetailersRequestDTO, CancellationToken cancellationToken);

        /// <summary>
        /// </summary>
        /// <param name="updateRetailerRequestDTO"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="RequiredRecordNotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        Task UpdateRetailerAsync(UpdateRetailerRequestDTO updateRetailerRequestDTO, CancellationToken cancellationToken);

        Task DeleteRetailerAsync(DeleteRetailerRequestDTO deleteRetailerRequestDTO, CancellationToken cancellationToken);

        Task<CreateRetailerResponseDTO> CreateRetailerAsync(CreateRetailerRequestDTO createRetailerRequestDTO, CancellationToken cancellationToken);
    }
}
