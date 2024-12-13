using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Request
{
    public class CreateUserRequestWebApiModel
    {
        [Required]
        [StringLength(16, ErrorMessage = "Invalid username character length", MinimumLength = 4)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "Invalid password character length", MinimumLength = 4)]
        public string Password { get; set; }
    }
}
