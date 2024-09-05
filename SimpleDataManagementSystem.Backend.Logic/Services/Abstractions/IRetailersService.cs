using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Abstractions
{
    public interface IRetailersService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newRetailerDTO"></param>
        /// <exception cref="RecordExistsException"></exception>
        /// <returns></returns>
        Task<int> AddNewRetailerAsync(NewRetailerDTO newRetailerDTO);
        Task<RetailersDTO?> GetAllRetailersAsync(int? take = 8, int? page = 1);
        Task<RetailerDTO?> GetRetailerByIdAsync(int retailerId);
        Task UpdateRetailerAsync(int retailerId, UpdateRetailerDTO updateRetailerDTO);
        Task DeleteRetailerAsync(int retailerId);
        Task UpdateRetailerPartialAsync(int retailerId);
    }
}
