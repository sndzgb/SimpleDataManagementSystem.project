using SimpleDataManagementSystem.Shared.Web.Validators;
using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write
{
    public class UpdateRetailerViewModel
    {
        [Required(ErrorMessage = "Retailer name is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid retailer name", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Retailer priority is required")]
        public int Priority { get; set; }

        [MaxFileSizeValidator(8 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is {0} bytes")]
        [AllowedExtensionsValidator(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? LogoImage { get; set; }
    }
}
