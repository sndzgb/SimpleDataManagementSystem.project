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
        Task<User?> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO);
        //Task UpdateUserAsync(int userId, User user);
        Task UpdatePasswordAsync(int userId, UpdatePasswordDTO updatePasswordDTO);
        //Task UpdateUserRole(int userId, int newRoleId);
        //Task UpdateUsername(int userId, string newUsername);
        Task DeleteUserAsync(int userId);
        Task<User?> GetUserByLogInCredentialsAsync(string username, string password);
    }
}
