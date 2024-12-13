using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.Controllers.Base;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Request;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Response;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Data;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUsersCoreService _usersCoreService;


        public UsersController(
                IAuthorizationService authorizationService,
                IUsersCoreService usersCoreService
            )
        {
            _authorizationService = authorizationService;
            _usersCoreService = usersCoreService;
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetSingleUser([FromRoute] int userId, CancellationToken cancellationToken)
        {
            var canGetSingleUser = await CanGetSingleUserAsync(HttpContext);

            if (!canGetSingleUser)
            {
                return Forbid();
            }

            var user = await _usersCoreService.GetUserAsync(new Logic.DTOs.Request.GetUserRequestDTO()
            {
                UserId = userId
            }, cancellationToken);

            if (user == null) 
            {
                return NotFound();
            }

            var response = MapToGetSingleUserResponse(user);

            return Ok(response);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int userId, CancellationToken cancellationToken)
        {
            var canDeleteUser = await CanDeleteUserAsync(HttpContext);

            if (!canDeleteUser)
            {
                return Forbid();
            }

            await _usersCoreService.DeleteUserAsync(new Logic.DTOs.Request.DeleteUserRequestDTO(
                    new Logic.DTOs.Request.DeleteUserRequestDTO.DeleteUserRequestMetadata(userId)
                )
            {
                RequestedByUserId = GetUserId()
            }, cancellationToken);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(
                [FromRoute] int userId, 
                [FromBody] UpdateUserRequestWebApiModel updateUserRequestWebApiModel, 
                CancellationToken cancellationToken
            )
        {
            var canUpdateUser = await CanUpdateUserAsync(HttpContext);

            if (!canUpdateUser)
            {
                return Forbid();
            }

            await _usersCoreService.UpdateUserAsync(new Logic.DTOs.Request.UpdateUserRequestDTO(
                    new Logic.DTOs.Request.UpdateUserRequestDTO.UpdateUserRequestMetadata(userId)
                )
            {
                Role = (Roles)updateUserRequestWebApiModel.RoleId,
                RequestedByUserId = GetUserId(),
                Username = updateUserRequestWebApiModel.Username
            }, cancellationToken);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(
                [FromBody] CreateUserRequestWebApiModel createUserRequestWebApiModel,
                CancellationToken cancellationToken
            )
        {
            var canCreateUser = await CanCreateUserAsync(HttpContext);

            if (!canCreateUser)
            {
                return Forbid();
            }

            var createdUser = await _usersCoreService.CreateUserAsync(new Logic.DTOs.Request.CreateUserRequestDTO()
            {
                Password = createUserRequestWebApiModel.Password,
                Role = (Roles)createUserRequestWebApiModel.RoleId,
                Username = createUserRequestWebApiModel.Username
            }, cancellationToken);

            var response = MapToCreateUserResponse(createdUser);

            return Ok(response);
            //return CreatedAtAction(nameof(GetSingleUser), new { id = response.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetMultipleUsers(
                CancellationToken cancellationToken,
                [FromQuery] int? take = 8, 
                [FromQuery] int? page = 1
            )
        {
            var canGetMultipleUsers = await CanGetMultipleUsersAsync(HttpContext);

            if (!canGetMultipleUsers)
            {
                return Forbid();
            }

            var users = await _usersCoreService.GetUsersAsync(new Logic.DTOs.Request.GetUsersRequestDTO()
            {
                PageInfo = new Logic.DTOs.Request.GetUsersRequestDTO.PageDTO()
                {
                    Page = (int)page,
                    Take = (int)take
                }
            }, cancellationToken);

            var response = MapToGetMultipleUsersResponse(users);
            response.PageInfo.Page = (int)page;
            response.PageInfo.Take = (int)take;

            return Ok(response);
        }


        #region Permissions

        private async Task<bool> CanUpdateUserAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanDeleteUserAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanCreateUserAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanGetMultipleUsersAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanGetSingleUserAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        #endregion

        #region Mapping

        private GetMultipleUsersResponseWebApiModel MapToGetMultipleUsersResponse(GetUsersResponseDTO users)
        {
            var response = new GetMultipleUsersResponseWebApiModel();

            response.PageInfo = new GetMultipleUsersResponseWebApiModel.PageWebApiModel()
            {
                Total = users.PageInfo.Total
            };

            response.Users = new List<GetMultipleUsersResponseWebApiModel.UserWebApiModel>();

            response.Users.AddRange(users.Users.Select(x => new GetMultipleUsersResponseWebApiModel.UserWebApiModel()
            {
                CreatedUTC = x.CreatedUtc,
                Id = x.Id,
                IsPasswordChangeRequired = x.IsPasswordChangeRequired,
                Role = new GetMultipleUsersResponseWebApiModel.UserWebApiModel.RoleWebApiModel()
                {
                    Id = x.Role.Id,
                    Name = x.Role.Name
                },
                Username = x.Username
            }).ToList());

            return response;
        }

        private CreateUserResponseWebApiModel MapToCreateUserResponse(CreateUserResponseDTO createdUser)
        {
            var response = new CreateUserResponseWebApiModel()
            {
                RoleId = (int)createdUser.Role,
                Id = createdUser.Id,
                IsPasswordChangeRequired = createdUser.IsPasswordChangeRequired,
                Username = createdUser.Username
            };

            return response;
        }

        private GetSingleUserResponseWebApiModel MapToGetSingleUserResponse(GetUserResponseDTO user)
        {
            var response = new GetSingleUserResponseWebApiModel()
            {
                Username = user.Username,
                CreatedUTC = user.CreatedUtc,
                IsPasswordChangeRequired = user.IsPasswordChangeRequired,
                Id = user.Id,
                RoleName = user.Role.Name,
                RoleId = user.Role.Id
            };

            return response;
        }

        #endregion
    }
}
