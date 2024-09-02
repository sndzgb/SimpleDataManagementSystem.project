using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;


        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllRoles() 
        {
            var roles = await _rolesService.GetAllRolesAsync();

            return Ok(roles);
        }
    }
}
