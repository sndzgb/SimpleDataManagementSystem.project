using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write
{
    public class UpdateUserViewModel
    {
        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; }

        [StringLength(maximumLength: 16, ErrorMessage = "Invalid username", MinimumLength = 4)]
        public string Username { get; set; }
    }
}
