using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write
{
    public class NewUserWebApiModel
    {
        [Required]
        [StringLength(16, ErrorMessage = "Invalid username character length", MinimumLength = 4)]
        public string Username { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "Invalid password character length", MinimumLength = 4)]
        public string Password { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid role ID.")]
        public int RoleId { get; set; }
    }
}
