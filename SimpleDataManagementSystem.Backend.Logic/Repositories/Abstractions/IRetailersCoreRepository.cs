using SimpleDataManagementSystem.Backend.Logic.DTOs;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions
{
    public interface IRetailersCoreRepository
    {
        Task<Retailer?> GetRetailerAsync(GetRetailerRequestDTO getRetailerRequestDTO, CancellationToken cancellationToken);
        Task<RetailersDTO?> GetRetailersAsync(GetRetailersRequestDTO getRetailersRequestDTO, CancellationToken cancellationToken);
        
        Task UpdateRetailerAsync(UpdateRetailerRequestDTO updateRetailerRequestDTO, CancellationToken cancellationToken);

        Task DeleteRetailerAsync(DeleteRetailerRequestDTO deleteRetailerRequestDTO, CancellationToken cancellationToken);

        Task<Retailer> CreateRetailerAsync(CreateRetailerRequestDTO createRetailerRequestDTO, CancellationToken cancellationToken);
    }
}
