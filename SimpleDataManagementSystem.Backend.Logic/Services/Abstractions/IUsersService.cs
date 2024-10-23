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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newUserDTO"></param>
        /// <exception cref="RecordExistsException"></exception>
        /// <returns></returns>
        Task<int> AddNewUserAsync(NewUserDTO newUserDTO);
        Task<UsersDTO?> GetAllUsersAsync(int? take = 8, int? page = 1);
        Task<UserDTO?> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO);
        Task UpdatePasswordAsync(int userId, UpdatePasswordDTO updatePasswordDTO);
        Task DeleteUserAsync(int userId);
        Task<UserLogInResultDTO?> GetUserByLogInCredentialsAsync(string username, string password);
    }
}
