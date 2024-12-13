using SimpleDataManagementSystem.Shared.Web.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request
{
    public class UpdateItemViewModel
    {
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid opis", MinimumLength = 1)]
        public string? Opis { get; set; }

        [StringLength(maximumLength: 255, ErrorMessage = "Invalid datum akcije length", MinimumLength = 1)]
        public string? Datumakcije { get; set; }

        [Required(ErrorMessage = "Retailer is required")]
        public int RetailerId { get; set; }

        [Required(ErrorMessage = "Cijena is required")]
        [DecimalValidator]
        public string Cijena { get; set; }

        public bool DeleteCurrentURLdoslike { get; set; }

        [Required(ErrorMessage = "Kategorija is required")]
        public int Kategorija { get; set; }

        [MaxFileSizeValidator(8 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is {0} bytes")]
        [AllowedExtensionsValidator(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? URLdoslike { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsMonitoredByUser { get; set; }
    }
}
