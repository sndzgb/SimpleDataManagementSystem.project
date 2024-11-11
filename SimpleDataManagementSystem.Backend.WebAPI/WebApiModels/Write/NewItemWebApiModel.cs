using SimpleDataManagementSystem.Shared.Web.Validators;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write
{
    public class NewItemWebApiModel
    {
        public string Nazivproizvoda { get; set; }
        public string Opis { get; set; }
        public string? Datumakcije { get; set; }
        public int RetailerId { get; set; }

        [MaxFileSizeValidator(8 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is {0} bytes")]
        [AllowedExtensionsValidator(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? URLdoslike { get; set; }

        [DecimalValidator]
        public string Cijena { get; set; }
        public int Kategorija { get; set; }
    }
}
