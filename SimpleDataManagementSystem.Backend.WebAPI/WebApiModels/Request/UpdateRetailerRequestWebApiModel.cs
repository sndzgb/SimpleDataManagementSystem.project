using SimpleDataManagementSystem.Shared.Web.Validators;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Request
{
    public sealed class UpdateRetailerRequestWebApiModel
    {
        public string? Name { get; set; }
        public int Priority { get; set; }

        [MaxFileSizeValidator(8 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is {0} bytes")]
        [AllowedExtensionsValidator(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? LogoImage { get; set; }
        public bool DeleteCurrentLogoImage { get; set; }
    }
}
