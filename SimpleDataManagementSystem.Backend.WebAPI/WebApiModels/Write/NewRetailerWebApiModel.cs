using Microsoft.AspNetCore.Mvc;
//using SimpleDataManagementSystem.Backend.WebAPI.Validators;
using SimpleDataManagementSystem.Shared.Web.Validators;
using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write
{
    public class NewRetailerWebApiModel
    {
        [FromForm]
        public string Name { get; set; }

        [FromForm]
        public int Priority { get; set; }

        [MaxFileSizeValidator(8 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is {0} bytes")]
        [AllowedExtensionsValidator(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? LogoImage { get; set; }
    }
}
