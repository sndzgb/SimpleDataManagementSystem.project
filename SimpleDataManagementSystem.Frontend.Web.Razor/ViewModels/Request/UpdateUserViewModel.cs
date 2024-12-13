using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request
{
    public class UpdateUserViewModel
    {
        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(maximumLength: 16, ErrorMessage = "Invalid username", MinimumLength = 4)]
        public string Username { get; set; }
    }
}
