using SimpleDataManagementSystem.Backend.WebAPI.Validators;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write
{
    public class UpdateRetailerWebApiModel
    {
        public string Name { get; set; }
        public int Priority { get; set; }

        [MaxFileSizeValidator(8 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is {0} bytes")]
        public IFormFile? LogoImage { get; set; }
    }
}
