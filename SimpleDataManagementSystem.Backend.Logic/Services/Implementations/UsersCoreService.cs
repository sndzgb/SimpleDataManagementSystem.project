using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Exceptions;
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
    public class UsersCoreService : IUsersCoreService
    {
        private readonly IUsersCoreRepository _usersCoreRepository;


        public UsersCoreService(
                IUsersCoreRepository usersCoreRepository,
                IEmailService emailService
            )
        {
            _usersCoreRepository = usersCoreRepository;
        }


        public async Task<CreateUserResponseDTO> CreateUserAsync(CreateUserRequestDTO createUserRequestDTO, CancellationToken cancellationToken)
        {
            if (createUserRequestDTO.Role == Roles.Admin)
            {
                throw new NotAllowedException("Creating new admin users is not allowed.");
            }

            var newUser = await _usersCoreRepository.CreateUserAsync(
                createUserRequestDTO,
            //    new CreateUserRequestDTO()
            //{
            //    Password = createUserRequestDTO.Password,
            //    RequestedByUserId = createUserRequestDTO.RequestedByUserId,
            //    Role = createUserRequestDTO.Role,
            //    Username = createUserRequestDTO.Username
            //}, 
                cancellationToken
            );

            var response = new CreateUserResponseDTO()
            {
                Id = newUser.ID,
                Username = newUser.Username,
                Role = (Roles)newUser.Role.ID,
                IsPasswordChangeRequired = newUser.IsPasswordChangeRequired
            };

            return response;
        }

        public async Task DeleteUserAsync(DeleteUserRequestDTO deleteUserRequestDTO, CancellationToken cancellationToken)
        {
            await _usersCoreRepository.DeleteUserAsync(deleteUserRequestDTO, cancellationToken);
        }

        public async Task<GetUserResponseDTO?> GetUserAsync(GetUserRequestDTO getUserRequestDTO, CancellationToken cancellationToken)
        {
            var user = await _usersCoreRepository.GetUserAsync(getUserRequestDTO, cancellationToken);

            if (user == null)
            {
                return null;
            }

            var response = new GetUserResponseDTO()
            {
                Id = user.ID,
                IsPasswordChangeRequired = user.IsPasswordChangeRequired,
                Role = new GetUserResponseDTO.RoleDTO()
                {
                    Id = user.Role.ID,
                    Name = user.Role.Name
                },
                Username = user.Username
            };

            return response;
        }

        public async Task<GetUserByLoginCredentialsResponseDTO?> GetUserByLoginCredentialsAsync(
                GetUserByLoginCredentialsRequestDTO getUserByLoginCredentialsRequestDTO,
                CancellationToken cancellationToken
            )
        {
            var user = await _usersCoreRepository.GetUserByLoginCredentialsAsync(getUserByLoginCredentialsRequestDTO, cancellationToken);

            if (user == null)
            {
                return null;
            }

            var response = new GetUserByLoginCredentialsResponseDTO()
            {
                Roles = new string[] {
                    user.Role?.Name!
                },
                UserId = user.ID,
                Username = user.Username,
                IsPasswordChangeRequired = user.IsPasswordChangeRequired
            };

            return response;
        }

        public async Task<GetUsersResponseDTO> GetUsersAsync(GetUsersRequestDTO getUsersRequestDTO, CancellationToken cancellationToken)
        {
            var users = await _usersCoreRepository.GetUsersAsync(getUsersRequestDTO, cancellationToken);

            var response = new GetUsersResponseDTO();
            response.Users = new List<GetUsersResponseDTO.UserDTO>();

            if (users.Users != null && users.Users.Count > 0)
            {
                response.Users.AddRange(users.Users.Select(x => new GetUsersResponseDTO.UserDTO()
                {
                    Id = x.ID,
                    IsPasswordChangeRequired = x.IsPasswordChangeRequired,
                    Role = new GetUsersResponseDTO.UserDTO.RoleDTO()
                    {
                        Id = x.Role.ID,
                        Name = x.Role.Name
                    },
                    Username = x.Username
                }));
            }

            response.PageInfo = new GetUsersResponseDTO.PageDTO()
            {
                Total = users.PageInfo.Total
            };

            return response;
        }

        public async Task UpdatePasswordAsync(UpdatePasswordRequestDTO updatePasswordRequestDTO, CancellationToken cancellationToken)
        {
            if (updatePasswordRequestDTO == null)
            {
                throw new ArgumentNullException(nameof(updatePasswordRequestDTO));
            }

            var user = await _usersCoreRepository.GetUserAsync(new GetUserRequestDTO()
            {
                RequestedByUserId = updatePasswordRequestDTO.RequestMetadata.UserId
            }, cancellationToken);

            if (user == null)
            {
                throw new RequiredRecordNotFoundException();
            }

            await _usersCoreRepository.UpdatePasswordAsync(new UpdatePasswordRequestDTO(
                    new UpdatePasswordRequestDTO.UpdatePasswordRequestMetadata(updatePasswordRequestDTO.RequestMetadata.UserId)
                )
            {
                NewPassword = updatePasswordRequestDTO.NewPassword,
                OldPassword = updatePasswordRequestDTO.OldPassword
            }, cancellationToken);

            await _usersCoreRepository.UpdateUserAsync(new UpdateUserRequestDTO(
                    new UpdateUserRequestDTO.UpdateUserRequestMetadata(updatePasswordRequestDTO.RequestMetadata.UserId)
                )
            {
                RequestedByUserId = updatePasswordRequestDTO.RequestMetadata.UserId,
                Role = (Roles)user.Role.ID,
                Username = user.Username
            }, cancellationToken);
        }

        public async Task UpdateUserAsync(UpdateUserRequestDTO updateUserRequestDTO, CancellationToken cancellationToken)
        {
            var user = await _usersCoreRepository.GetUserAsync(new GetUserRequestDTO()
            {
                RequestedByUserId = updateUserRequestDTO.RequestedByUserId,
                UserId = updateUserRequestDTO.RequestMetadata.UserId
            }, cancellationToken);

            if (user == null)
            {
                throw new RequiredRecordNotFoundException();
            }

            await _usersCoreRepository.UpdateUserAsync(new UpdateUserRequestDTO(
                    new UpdateUserRequestDTO.UpdateUserRequestMetadata(updateUserRequestDTO.RequestMetadata.UserId)
                )
            {
                Role = updateUserRequestDTO.Role,
                Username = updateUserRequestDTO.Username
            }, cancellationToken);
        }
    }
}
