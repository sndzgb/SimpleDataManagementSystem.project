//using SimpleDataManagementSystem.Backend.WebAPI.Validators;
using SimpleDataManagementSystem.Shared.Web.Validators;
using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write
{
    public class UpdateItemWebApiModel
    {
        public string Opis { get; set; }
        public string Datumakcije { get; set; }
        public int RetailerId { get; set; }

        [DataType(DataType.Upload)]
        [AllowedExtensionsValidator(new string[] { ".jpg", ".jpeg", ".png" })]
        [MaxFileSizeValidator(10 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is {0} bytes")]
        public IFormFile? URLdoslike { get; set; }

        [DecimalValidator]
        public string Cijena { get; set; }
        public int Kategorija { get; set; }
    }
}
