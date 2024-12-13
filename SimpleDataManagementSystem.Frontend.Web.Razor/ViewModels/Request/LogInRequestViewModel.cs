using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request
{
    public class LogInRequestViewModel
    {
        [Required(ErrorMessage = "Username is a required field")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is a required field")]
        public string Password { get; set; }
    }
}
