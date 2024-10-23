using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.Policies;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Data;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")] // TODO
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IAuthorizationService _authorizationService;


        public UsersController(IUsersService usersService, IAuthorizationService authorizationService)
        {
            _usersService = usersService;
            _authorizationService = authorizationService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int? take = 8, int? page = 1)
        {
            //int[] roles = { (int)Roles.Admin }; // TODO put in class/ model
            //AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
            //    User,
            //    new { roles },
            //    Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            //);

            //if (!authorizationResult.Succeeded)
            //{
            //    return new ObjectResult
            //    (
            //        new ErrorWebApiModel(StatusCodes.Status403Forbidden, "You are not authorized to view this resource.", null)
            //    );
            //}

            int[] roles = new int[] { (int)Roles.Admin };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var users = await _usersService.GetAllUsersAsync(take, page);
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser([FromBody] NewUserWebApiModel newUserWebApiModel)
        {
            int[] roles = new int[] { (int)Roles.Admin };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var newUserId = await _usersService.AddNewUserAsync(new NewUserDTO()
            {
                Password = newUserWebApiModel.Password,
                RoleId = newUserWebApiModel.RoleId,
                Username = newUserWebApiModel.Username
            });

            // TODO get newly created user from DB, and return it to the client
            return Created($"api/users/{newUserId}", newUserId);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            int[] roles = new int[] { (int)Roles.Admin };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var user = await _usersService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new ErrorWebApiModel(StatusCodes.Status404NotFound, "The requested resource was not found.", null));
            }

            return Ok(user);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserWebApiModel updateUserWebApiModel)
        {
            int[] roles = new int[] { (int)Roles.Admin };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _usersService.UpdateUserAsync(userId, new UpdateUserDTO()
            {
                RoleId = updateUserWebApiModel.RoleId,
                Username = updateUserWebApiModel.Username
            });

            return Ok();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            int[] roles = new int[] { (int)Roles.Admin };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _usersService.DeleteUserAsync(userId);

            return Ok();
        }

        [HttpPut("{userId}/password")]
        [AllowAnonymous]
        [Obsolete(message: "Use 'AccountsController' password change method.")]
        public async Task<IActionResult> UpdatePassword(int userId, UpdatePasswordWebApiModel updatePasswordWebApiModel)
        {
            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                userId,
                Shared.Common.Constants.Policies.PolicyNames.UserIsResourceOwner
            );

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _usersService.UpdatePasswordAsync(userId, new UpdatePasswordDTO()
            {
                NewPassword = updatePasswordWebApiModel.NewPassword,
                OldPassword = updatePasswordWebApiModel.OldPassword
            });

            return Ok();
        }
    }
}
