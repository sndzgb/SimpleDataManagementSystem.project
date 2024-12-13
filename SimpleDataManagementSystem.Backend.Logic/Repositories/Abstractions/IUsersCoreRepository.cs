using SimpleDataManagementSystem.Backend.Logic.DTOs;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions
{
    public interface IUsersCoreRepository
    {
        Task<User?> GetUserAsync(GetUserRequestDTO getUserRequestDTO, CancellationToken cancellationToken);
        Task<UsersDTO> GetUsersAsync(GetUsersRequestDTO getUsersRequestDTO, CancellationToken cancellationToken);

        Task DeleteUserAsync(DeleteUserRequestDTO deleteUserRequestDTO, CancellationToken cancellationToken);
        Task UpdateUserAsync(UpdateUserRequestDTO updateUserRequestDTO, CancellationToken cancellationToken);

        Task UpdatePasswordAsync(UpdatePasswordRequestDTO updatePasswordRequestDTO, CancellationToken cancellationToken);

        Task<User> CreateUserAsync(CreateUserRequestDTO createUserRequestDTO, CancellationToken cancellationToken);

        Task<User?> GetUserByLoginCredentialsAsync(
            GetUserByLoginCredentialsRequestDTO getUserByLoginCredentialsRequestDTO, 
            CancellationToken cancellationToken
        );
    }
}
