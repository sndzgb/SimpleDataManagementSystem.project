using SimpleDataManagementSystem.Frontend.Web.Razor.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write
{
    public class UpdateItemViewModel
    {
        [Required(ErrorMessage = "Naziv proizvoda is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid naziv proizvoda name", MinimumLength = 2)]
        public string Nazivproizvoda { get; set; }

        [Required(ErrorMessage = "Opis is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid opis", MinimumLength = 2)]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Datum akcije name is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid datum akcije", MinimumLength = 2)]
        public string Datumakcije { get; set; }

        [Required(ErrorMessage = "Naziv retailera is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid naziv retailera", MinimumLength = 2)]
        public string Nazivretailera { get; set; }

        //[DataType(DataType.Currency)]
        //[DecimalValidator]
        //[DisplayName("Price")]
        public string Cijena { get; set; }
        public int Kategorija { get; set; }

        public IFormFile? URLdoslike { get; set; }
    }
}
