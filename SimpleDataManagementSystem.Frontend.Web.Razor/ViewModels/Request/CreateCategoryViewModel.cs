using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request
{
    public class CreateCategoryViewModel
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid category name length", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public int? Priority { get; set; }
    }
}
