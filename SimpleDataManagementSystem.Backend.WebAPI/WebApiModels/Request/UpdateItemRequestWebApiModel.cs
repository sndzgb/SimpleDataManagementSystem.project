using SimpleDataManagementSystem.Shared.Web.Validators;
using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Request
{
    public sealed class UpdateItemRequestWebApiModel
    {
        [DecimalValidator]
        public decimal Cijena { get; set; }
        public int RetailerId { get; set; }
        public string? Datumakcije { get; set; }
        public string? Opis { get; set; }
        public bool IsEnabled { get; set; }
        public int Kategorija { get; set; }

        //[DataType(DataType.Upload)]
        [AllowedExtensionsValidator(new string[] { ".jpg", ".jpeg", ".png" })]
        [MaxFileSizeValidator(10 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is {0} bytes")]
        public IFormFile? URLdoslike { get; set; }
        public bool DeleteCurrentURLdoslike { get; set; }
        public bool IsMonitoredByUser { get; set; }
    }
}
