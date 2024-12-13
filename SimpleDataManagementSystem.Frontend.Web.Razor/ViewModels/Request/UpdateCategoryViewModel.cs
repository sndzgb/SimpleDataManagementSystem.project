using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request
{
    public class UpdateCategoryViewModel
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid category name", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category priority is required")]
        [Range(int.MinValue, int.MaxValue)]
        public int Priority { get; set; }
    }
}
