using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Abstractions
{
    public interface IUsersService
    {
        Task<int> AddNewUserAsync(NewUserDTO newUserDTO);
        Task<List<UserDTO>> GetAllUsersAsync(int? take = 8, int? page = 1);
        Task<UserDTO?> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO);
        Task DeleteUserAsync(int userId);
    }
}
