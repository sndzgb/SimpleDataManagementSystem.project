﻿using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write
{
    public class UpdateCategoryViewModel
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "Invalid category name", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category priority is required")]
        [Range(Int32.MinValue, Int32.MaxValue)]
        public int Priority { get; set; }
    }
}
