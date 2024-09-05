using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions
{
    public interface IUsersRepository
    {
        Task<int> AddNewUserAsync(NewUserDTO newUserDTO);
        Task<UsersDTO?> GetAllUsersAsync(int? take = 8, int? page = 1);
        Task<UserDTO?> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO);
        Task DeleteUserAsync(int userId);
        Task<User?> GetUserByLogInCredentialsAsync(string username, string password);
    }
}
