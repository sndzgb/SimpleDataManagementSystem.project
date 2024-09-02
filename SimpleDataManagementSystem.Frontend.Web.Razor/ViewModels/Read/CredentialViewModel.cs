using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read
{
    public class CredentialViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
