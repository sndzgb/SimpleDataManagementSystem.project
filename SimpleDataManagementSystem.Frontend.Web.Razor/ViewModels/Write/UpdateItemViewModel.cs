using SimpleDataManagementSystem.Shared.Web.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write
{
    public class UpdateItemViewModel
    {
        [Required(ErrorMessage = "Opis is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid opis", MinimumLength = 2)]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Datum akcije name is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid datum akcije", MinimumLength = 2)]
        public string Datumakcije { get; set; }

        [Required(ErrorMessage = "Retailer is required")]
        public int RetailerId { get; set; }

        [DecimalValidator]
        public string Cijena { get; set; }
        public int Kategorija { get; set; }

        [MaxFileSizeValidator(8 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is {0} bytes")]
        [AllowedExtensionsValidator(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? URLdoslike { get; set; }
    }
}
