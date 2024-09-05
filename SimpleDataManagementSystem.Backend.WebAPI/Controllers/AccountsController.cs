using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Records;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ITokenGeneratorService _tokenGeneratorService;


        public AccountsController(IUsersService usersService, ITokenGeneratorService tokenGeneratorService)
        {
            _usersService = usersService;
            _tokenGeneratorService = tokenGeneratorService;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(UserLogInRequestDTO userLogInRequestDTO)
        {
            var user = await _usersService.GetUserByLogInCredentialsAsync(userLogInRequestDTO.Username, userLogInRequestDTO.Password);

            if (user == null) 
            {
                return Unauthorized();
            }

            var jwt = await _tokenGeneratorService.GenerateTokenAsync(
                new AuthenticatedUser
                (
                    user.UserId,
                    user.Username,
                    user.Roles
                )
            );

            return Ok(jwt);
        }
    }
}
