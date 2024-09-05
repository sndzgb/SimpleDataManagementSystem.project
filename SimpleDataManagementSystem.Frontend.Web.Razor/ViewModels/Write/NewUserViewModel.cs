using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write
{
    public class NewUserViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(16, ErrorMessage = "Invalid username length", MinimumLength = 4)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(16, ErrorMessage = "Invalid password length", MinimumLength = 4)]
        public string Password { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid role")]
        public int RoleId { get; set; }
    }
}
