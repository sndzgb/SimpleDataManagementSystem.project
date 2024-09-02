using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write
{
    public class NewRetailerViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid name", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public int Priority { get; set; }

        [IgnoreDataMember]
        public IFormFile? LogoImage { get; set; }
    }
}
