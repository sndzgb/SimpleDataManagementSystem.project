using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write
{
    public class NewCategoryViewModel
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid category name", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public int Priority { get; set; }
    }
}
