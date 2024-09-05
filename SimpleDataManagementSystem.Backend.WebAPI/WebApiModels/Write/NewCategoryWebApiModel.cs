using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write
{
    public class NewCategoryWebApiModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public int Priority { get; set; }
    }
}
