using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write
{
    public class NewUserViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(16, ErrorMessage = "Invalid username", MinimumLength = 4)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid role ID.")]
        public int RoleId { get; set; }
    }
}
