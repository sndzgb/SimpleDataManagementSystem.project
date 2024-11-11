using SimpleDataManagementSystem.Shared.Web.Validators;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write
{
    public class NewItemViewModel
    {
        [Required(ErrorMessage = "Item name is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid item name", MinimumLength = 2)]
        public string Nazivproizvoda { get; set; }

        //[Required(ErrorMessage = "Opis is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Opis is invalid")]
        public string? Opis { get; set; }

        //[Required(ErrorMessage = "Datum akcije is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid datum akcije", MinimumLength = 2)]
        public string? Datumakcije { get; set; }

        [Required(ErrorMessage = "Retailer is required")]
        public int? RetailerId { get; set; }

        [Required]
        [DecimalValidator]
        public string Cijena { get; set; }

        [Required(ErrorMessage = "Kategorija is required")]
        [MaxLength(255, ErrorMessage = "Kategorija invalid max value")]
        [MinLength(0, ErrorMessage = "Kategorija invalid min value")]
        public int? Kategorija { get; set; }

        [IgnoreDataMember]
        [MaxFileSizeValidator(8 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is {0} bytes")]
        [AllowedExtensionsValidator(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? URLdoslike { get; set; }
    }
}
