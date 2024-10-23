using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write
{
    public class UpdatePasswordWebApiModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Old password must not be empty")]
        public string OldPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "New password must not be empty")]
        public string NewPassword { get; set; }
    }
}
