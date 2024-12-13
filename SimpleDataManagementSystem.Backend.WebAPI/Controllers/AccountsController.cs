using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.Controllers.Base;
using SimpleDataManagementSystem.Backend.WebAPI.Helpers;
using SimpleDataManagementSystem.Backend.WebAPI.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Records;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Request;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController
    {
        private readonly IUsersCoreService _usersCoreService;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly IAuthorizationService _authorizationService;


        public AccountsController(IUsersCoreService usersCoreService, ITokenGeneratorService tokenGeneratorService,
            IAuthorizationService authorizationService)
        {
            _usersCoreService = usersCoreService;
            _tokenGeneratorService = tokenGeneratorService;
            _authorizationService = authorizationService;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(
                GetUserByLoginCredentialsRequestWebApiModel getUserByLoginCredentialsRequestWebApiModel,
                CancellationToken cancellationToken
            )
        {
            var user = await _usersCoreService.GetUserByLoginCredentialsAsync(new GetUserByLoginCredentialsRequestDTO()
            {
                Password = getUserByLoginCredentialsRequestWebApiModel.Password,
                Username = getUserByLoginCredentialsRequestWebApiModel.Username
            }, cancellationToken);

            if (user == null)
            {
                return Unauthorized(new ErrorWebApiModel((int)HttpStatusCode.Unauthorized, "Invalid username and / or password.", null));
            }

            var jwt = await _tokenGeneratorService.GenerateTokenAsync(
                new AuthenticatedUser
                (
                    user.UserId,
                    user.Username,
                    user.Roles,
                    user.IsPasswordChangeRequired
                ), 
                cancellationToken
            );

            return Ok(new { jwt = jwt });
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetAccountDetails(CancellationToken cancellationToken)
        {
            var canGetAccountDetails = await CanGetAccountDetailyAsync(HttpContext);

            if (!canGetAccountDetails)
            {
                return Forbid();
            }

            var user = await _usersCoreService.GetUserAsync(new Logic.DTOs.Request.GetUserRequestDTO()
            {
                RequestedByUserId = GetUserId(),
                UserId = GetUserId()
            }, cancellationToken);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut("password")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordWebApiModel passwordChangeWebApiModel, CancellationToken cancellationToken)
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

            await _usersCoreService.UpdatePasswordAsync(new UpdatePasswordRequestDTO(
                    new UpdatePasswordRequestDTO.UpdatePasswordRequestMetadata(GetUserId())
                )
            {
                OldPassword = passwordChangeWebApiModel.OldPassword,
                NewPassword = passwordChangeWebApiModel.NewPassword
            }, cancellationToken);

            return Ok();
        }


        #region Permissions

        private async Task<bool> CanGetAccountDetailyAsync(HttpContext httpContext)
        {
            var userId = HttpContextHelpers.GetAuthenticatedUserIdFromHttpContext(HttpContext);

            if (userId == null)
            {
                return false;
            }

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                GetUserId(),
                Shared.Common.Constants.Policies.PolicyNames.UserIsResourceOwner // "UserIsResourceOwnerPolicy"
            );

            return authorizationResult.Succeeded;
        }

        #endregion
    }
}
