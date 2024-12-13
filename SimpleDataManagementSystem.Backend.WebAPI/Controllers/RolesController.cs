using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Response;
using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesCoreService _rolesCoreService;

        public RolesController(IRolesCoreService rolesCoreService)
        {
            _rolesCoreService = rolesCoreService;
        }


        [HttpGet]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
        {
            var roles = await _rolesCoreService.GetRolesAsync(cancellationToken);

            var response = MapToGetRolesResponse(roles);

            return Ok(response);
        }


        #region Mapping

        private GetRolesWebApiModel MapToGetRolesResponse(GetRolesResponseDTO getRolesResponseDTO)
        {
            var response = new GetRolesWebApiModel();
            response.Roles = new List<GetRolesWebApiModel.RoleWebApiModel>();
            response.Roles.AddRange(getRolesResponseDTO.Roles.Select(x => new GetRolesWebApiModel.RoleWebApiModel()
            {
                Id = x.Id,
                Name = x.Name
            }));

            response.PageInfo = new GetRolesWebApiModel.PageWebApiModel()
            {
                Page = 1,
                Take = 8
            };

            return response;
        }

        #endregion
    }
}
