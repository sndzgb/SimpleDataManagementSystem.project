using SimpleDataManagementSystem.Shared.Web.Validators;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request
{
    public class CreateRetailerViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid name", MinimumLength = 2)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public int? Priority { get; set; }

        [JsonIgnore]
        [MaxFileSizeValidator(4 * 1024 * 1024)]
        [AllowedExtensionsValidator(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? LogoImage { get; set; }
    }
}
