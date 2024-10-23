using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.Helpers;
using SimpleDataManagementSystem.Backend.WebAPI.Policies;
using SimpleDataManagementSystem.Backend.WebAPI.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Records;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly IAuthorizationService _authorizationService;


        public AccountsController(IUsersService usersService, ITokenGeneratorService tokenGeneratorService,
            IAuthorizationService authorizationService)
        {
            _usersService = usersService;
            _tokenGeneratorService = tokenGeneratorService;
            _authorizationService = authorizationService;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(UserLogInRequestDTO userLogInRequestDTO)
        {
            var user = await _usersService.GetUserByLogInCredentialsAsync(userLogInRequestDTO.Username, userLogInRequestDTO.Password);

            if (user == null)
            {
                return NotFound(new ErrorWebApiModel((int)HttpStatusCode.NotFound, "User not found.", null));
            }

            var jwt = await _tokenGeneratorService.GenerateTokenAsync(
                new AuthenticatedUser
                (
                    user.UserId,
                    user.Username,
                    user.Roles,
                    user.IsPasswordChangeRequired
                )
            );

            return Ok(new { jwt = jwt });
        }

        [HttpGet("details")]
        //[Route("details")]
        //[AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> GetAccountDetails()
        {
            var userId = HttpContextHelpers.GetAuthenticatedUserIdFromHttpContext(HttpContext);

            if (userId == null)
            {
                return BadRequest();
            }

            //return Forbid();
            //return Unauthorized(new ErrorWebApiModel(StatusCodes.Status401Unauthorized, "Unauthorized to view this resource"));

            var user = await _usersService.GetUserByIdAsync((int)userId);

            if (user == null)
            {
                return NotFound();
            }

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                user.ID,
                Shared.Common.Constants.Policies.PolicyNames.UserIsResourceOwner // "UserIsResourceOwnerPolicy"
            );

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            return Ok(user);
        }

        [HttpPut("password")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordWebApiModel passwordChangeWebApiModel)
        {
            // allowAnonymous is just for the purposes of "PasswordChangeRequiredCheckMiddleware"
            // user has to be authenticated, and if password change is required - let the user exectue this api call
            if (!HttpContext.User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userId = HttpContextHelpers.GetAuthenticatedUserIdFromHttpContext(HttpContext);

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                userId,
                Shared.Common.Constants.Policies.PolicyNames.UserIsResourceOwner // "UserIsResourceOwnerPolicy"
            );

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _usersService.UpdatePasswordAsync((int)userId!, new UpdatePasswordDTO()
            {
                OldPassword = passwordChangeWebApiModel.OldPassword,
                NewPassword = passwordChangeWebApiModel.NewPassword
            });

            return Ok();
        }
    }
}
