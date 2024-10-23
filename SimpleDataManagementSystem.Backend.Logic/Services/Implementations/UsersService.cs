//using SimpleDataManagementSystem.Backend.Logic.Constants;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Exceptions;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;


        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }


        public async Task<int> AddNewUserAsync(NewUserDTO newUserDTO)
        {
            if (newUserDTO == null)
            {
                throw new ArgumentNullException(nameof(newUserDTO));
            }

            if (newUserDTO.RoleId == (int)Roles.Admin)
            {
                throw new NotAllowedException("Creating new admin accounts is not allowed.");
            }

            return await _usersRepository.AddNewUserAsync(newUserDTO);
        }

        public async Task<UsersDTO?> GetAllUsersAsync(int? take = 8, int? page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (take < 1)
            {
                take = 8;
            }

            return await _usersRepository.GetAllUsersAsync(take, page);
        }

        public async Task<UserDTO?> GetUserByIdAsync(int userId)
        {
            var user = await _usersRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            var userDTO = new UserDTO()
            {
                ID = userId,
                RoleId = user.Role?.ID,
                RoleName = user.Role?.Name,
                Username = user.Username,
                IsPasswordChangeRequired = user.IsPasswordChangeRequired
            };

            return userDTO;
        }

        public async Task UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO)
        {
            if (updateUserDTO == null)
            {
                return;
            }

            var user = await _usersRepository.GetUserByIdAsync(userId);

            if (user == null) 
            {
                return;
            }

            if ((updateUserDTO.RoleId == (int)Roles.Admin) && (user.Role?.ID != (int)Roles.Admin)) // not allowed to add new admin roles
            {
                return;
            }

            await _usersRepository.UpdateUserAsync(userId, updateUserDTO);
            //await _usersRepository.UpdateUserAsync(userId, new User()
            //{
            //    Role = new Role()
            //    {
            //        ID = updateUserDTO.RoleId
            //    },
            //    Username = updateUserDTO.Username,
            //    IsPasswordChangeRequired = updateUserDTO.IsPasswordChangeRequired // admin CAN set manually
            //});

            return;
        }

        public async Task UpdatePassword(int userId, UpdatePasswordDTO updatePasswordDTO)
        {
            await _usersRepository.UpdatePasswordAsync(userId, updatePasswordDTO);
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _usersRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return;
            }

            if (user.Role?.ID == (int)Roles.Admin) // not allowed to delete "admin" account
            {
                return;
            }

            await _usersRepository.DeleteUserAsync(userId);

            return;
        }

        // TODO move to accountsService
        public async Task<UserLogInResultDTO?> GetUserByLogInCredentialsAsync(string username, string password)
        {
            var user = await _usersRepository.GetUserByLogInCredentialsAsync(username, password);

            if (user == null)
            {
                return null;
            }

            var userDTO = new UserLogInResultDTO()
            {
                Roles = new string[] {
                    user.Role?.Name!
                },
                UserId = user.ID,
                Username = user.Username,
                IsPasswordChangeRequired = user.IsPasswordChangeRequired
            };

            return userDTO;
        }

        public async Task UpdatePasswordAsync(int userId, UpdatePasswordDTO updatePasswordDTO)
        {
            if (updatePasswordDTO == null)
            {
                throw new NotAllowedException("The provided values are not valid.");
            }

            var user = await _usersRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new RequiredRecordNotFoundException("The requested user was not found.");
            }

            await _usersRepository.UpdatePasswordAsync(userId, updatePasswordDTO);

            await _usersRepository.UpdateUserAsync(userId, new UpdateUserDTO()
            {
                IsPasswordChangeRequired = false,
                RoleId = user.Role?.ID,
                Username = user.Username
            });

            //user.IsPasswordChangeRequired = false;
            //await _usersRepository.UpdateUserAsync(userId, new User()
            //{
                
            //});
        }
    }
}
