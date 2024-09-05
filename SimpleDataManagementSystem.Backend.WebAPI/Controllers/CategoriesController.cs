using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write;

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
        public async Task<IActionResult> AddNewCategory([FromBody] NewCategoryWebApiModel newCategoryWebApiModel)
        {
            var newCategoryId = await _categoriesService.AddNewCategoryAsync(new NewCategoryDTO()
            {
                Name = newCategoryWebApiModel.Name,
                Priority = newCategoryWebApiModel.Priority
            });

            // TODO get newly created category from DB, and return it to the client
            return Created($"api/categories/{newCategoryId}", newCategoryId);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var category = await _categoriesService.GetCategoryByIdAsync(categoryId);

            if (category == null)
            {
                return NotFound(new ErrorWebApiModel(StatusCodes.Status404NotFound, "The requested resource was not found.", null));
            }

            return Ok(category);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, UpdateCategoryWebApiModel updateCategoryWebApiModel)
        {
            await _categoriesService.UpdateCategoryAsync(categoryId, new UpdateCategoryDTO()
            {
                Name = updateCategoryWebApiModel.Name,
                Priority = updateCategoryWebApiModel.Priority
            });

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
