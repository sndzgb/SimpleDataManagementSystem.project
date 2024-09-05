using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;


        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int? take = 8, int? page = 1)
        {
            var users = await _usersService.GetAllUsersAsync(take, page);
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser([FromBody] NewUserWebApiModel newUserWebApiModel)
        {
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
            await _usersService.DeleteUserAsync(userId);

            return Ok();
        }
    }
}
