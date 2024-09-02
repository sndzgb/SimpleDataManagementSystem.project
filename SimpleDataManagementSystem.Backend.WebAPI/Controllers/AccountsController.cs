using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;


        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }


        [HttpPost]
        public async Task<IActionResult> LogIn(UserLogInRequestDTO userLogInRequestDTO)
        {
            var userDTO = await _accountsService.LogInAsync(userLogInRequestDTO.Username, userLogInRequestDTO.Password);

            return Ok(userDTO);
        }
    }
}
