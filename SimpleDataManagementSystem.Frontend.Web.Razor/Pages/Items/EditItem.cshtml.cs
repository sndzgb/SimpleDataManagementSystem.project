using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Items
{
    [Authorize(Roles = "Admin,Employee")]
    public class EditItemModel : PageModel
    {
        private readonly IItemsService _itemsService;
        private readonly ICategoriesService _categoriesService;


        public EditItemModel(IItemsService itemsService, ICategoriesService categoriesService)
        {
            _itemsService = itemsService;
            _categoriesService = categoriesService;
        }


        public string ItemId { get; set; }

        public ItemViewModel Item { get; set; }

        [BindProperty]
        public UpdateItemViewModel UpdatedItem { get; set; }

        public List<CategoryViewModel> AvailableCategories { get; set; }


        public async Task OnGet(string itemId)
        {
            ItemId = itemId;

            var tasks = new List<Task>();

            tasks.Add(Task.Run(async () => Item = await _itemsService.GetItemByIdAsync(itemId)));
            tasks.Add(Task.Run(async () => AvailableCategories = await _categoriesService.GetAllCategoriesAsync(int.MaxValue, 1)));

            await Task.WhenAll(tasks);

            return;
        }

        public async Task<IActionResult> OnPost([FromRoute] string itemId)
        {
            //try
            //{
                ItemId = itemId;

                //var valid = TryValidateModel(UpdatedItem);

                //var errors = ModelState.Select(x => x.Value?.Errors)
                //               .Where(y => y.Count > 0)
                //               .ToList();

                await _itemsService.UpdateItemAsync(ItemId, UpdatedItem);

                return RedirectToPage("/Items/Items");
            //}
            //catch (WebApiCallException wace)
            //{
                // TODO display error(s)

                //ViewData["Error"] = wace.Error;
                //return RedirectToPage($"/Items/{ItemId}/Edit", new { ItemId = itemId });
            //}
        }
    }
}
