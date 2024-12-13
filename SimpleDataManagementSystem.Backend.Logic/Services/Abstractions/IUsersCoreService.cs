using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Exceptions;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Abstractions
{
    public interface IUsersCoreService
    {
        Task<GetUserByLoginCredentialsResponseDTO?> GetUserByLoginCredentialsAsync(
            GetUserByLoginCredentialsRequestDTO getUserByLoginCredentialsRequestDTO, 
            CancellationToken cancellationToken
        );

        Task UpdatePasswordAsync(UpdatePasswordRequestDTO updatePasswordRequestDTO, CancellationToken cancellationToken);

        Task<GetUserResponseDTO?> GetUserAsync(GetUserRequestDTO getUserRequestDTO, CancellationToken cancellationToken);
        Task<GetUsersResponseDTO?> GetUsersAsync(GetUsersRequestDTO getUsersRequestDTO, CancellationToken cancellationToken);

        Task DeleteUserAsync(DeleteUserRequestDTO deleteUserRequestDTO, CancellationToken cancellationToken);
        Task UpdateUserAsync(UpdateUserRequestDTO updateUserRequestDTO, CancellationToken cancellationToken);

        Task<CreateUserResponseDTO> CreateUserAsync(CreateUserRequestDTO createUserRequestDTO, CancellationToken cancellationToken);
    }
}
