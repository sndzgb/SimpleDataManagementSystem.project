using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write
{
    public class UpdateUserWebApiModel
    {
        [Required]
        public int RoleId { get; set; }
        
        [Required]
        public string Username { get; set; }
    }
}
