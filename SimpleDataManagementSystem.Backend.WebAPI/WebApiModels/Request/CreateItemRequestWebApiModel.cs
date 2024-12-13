using SimpleDataManagementSystem.Shared.Web.Validators;
using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Request
{
    public sealed class CreateItemRequestWebApiModel
    {
        [Required(ErrorMessage = "Nazivproizvoda is a required field.")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid Nazivproizvoda field length.", MinimumLength = 2)]
        public string Nazivproizvoda { get; set; }

        [MaxFileSizeValidator(8 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is {0} bytes")]
        [AllowedExtensionsValidator(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? URLdoslike { get; set; }
        public string? Opis { get; set; }
        public string? Datumakcije { get; set; }

        [DecimalValidator]
        public decimal Cijena { get; set; }
        public int Kategorija { get; set; }
        public int RetailerId { get; set; }
        public bool IsEnabled { get; set; }
    }
}
