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

        // TODO validate file size/ length using custom data annotation
        public IFormFile? LogoImage { get; set; }
    }
}
