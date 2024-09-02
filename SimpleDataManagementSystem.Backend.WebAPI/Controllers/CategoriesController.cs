using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;


        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCategories(int? take = 8, int? page = 1)
        {
            var categories = await _categoriesService.GetAllCategoriesAsync(take, page);
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewCategory([FromBody] NewCategoryDTO newCategoryDTO)
        {
            var newCategoryId = await _categoriesService.AddNewCategoryAsync(newCategoryDTO);

            return Created($"api/categories/{newCategoryId}", newCategoryId);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var category = await _categoriesService.GetCategoryByIdAsync(categoryId);

            return Ok(category);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, UpdateCategoryDTO updateCategoryDTO)
        {
            await _categoriesService.UpdateCategoryAsync(categoryId, updateCategoryDTO);

            return Ok();
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            await _categoriesService.DeleteCategoryAsync(categoryId);

            return Ok();
        }
    }
}
