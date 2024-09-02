using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Implementations
{
    public class RetailersService : IRetailersService
    {
        private readonly IRetailersRepository _retailersRepository;


        public RetailersService(IRetailersRepository retailersRepository)
        {
            _retailersRepository = retailersRepository;
        }


        public async Task<int> AddNewRetailerAsync(NewRetailerDTO newRetailerDTO)
        {
            return await _retailersRepository.AddNewRetailerAsync(newRetailerDTO);
        }

        public async Task DeleteRetailerAsync(int retailerId)
        {
            await _retailersRepository.DeleteRetailerAsync(retailerId);
        }

        public async Task<List<RetailerDTO>> GetAllRetailersAsync(int? take = 8, int? page = 1)
        {
            return await _retailersRepository.GetAllRetailersAsync(take, page);
        }

        public async Task<RetailerDTO?> GetRetailerByIdAsync(int retailerId)
        {
            return await _retailersRepository.GetRetailerByIdAsync(retailerId);
        }

        public async Task UpdateRetailerAsync(int retailerId, UpdateRetailerDTO updateRetailerDTO)
        {
            await _retailersRepository.UpdateRetailerAsync(retailerId, updateRetailerDTO);
        }
    }
}
