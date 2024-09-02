using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using System.Net;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Items
{
    [IgnoreAntiforgeryToken]
    [Authorize(Roles = "Admin,Employee")]
    public class DeleteItemModel : PageModel
    {
        private readonly IItemsService _itemsService;


        public DeleteItemModel(IItemsService itemsService)
        {
            _itemsService = itemsService;
        }


        public string ItemId { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostDeleteItem(string itemId)
        {
            ItemId = itemId;

            await _itemsService.DeleteItemAsync(ItemId);

            return new JsonResult(null) { StatusCode = (int)HttpStatusCode.OK };
        }
    }
}
