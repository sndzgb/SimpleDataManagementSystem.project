using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using System.Diagnostics;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Items
{
    [Authorize(Roles = "Admin,Employee")]
    [ValidateAntiForgeryToken]
    public class EditItemModel : PageModel
    {
        // TODO pageModel error handling
        // TRY-CATCH exception in "PageFilter"
        // base WebApiCallException property and populate in "PageFilter"
        // ViewComponent @await ... - modelState & error

        //public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        //{
        //    var isPost = context.HandlerMethod.HttpMethod.ToLower() == "post";

        //    if (isPost)
        //    {
        //        if (!context.ModelState.IsValid)
        //        {
        //            context.Result = new ObjectResult(null)
        //            {
        //                StatusCode = StatusCodes.Status400BadRequest
        //            };

        //            //context.HttpContext.Response.Redirect("/Items/123/Edit");
        //        }
        //    }
        //}


        private readonly IItemsService _itemsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IRetailersService _retailersService;
        private readonly ILogger<EditItemModel> _logger;


        public EditItemModel(IItemsService itemsService, ICategoriesService categoriesService, IRetailersService retailersService, ILogger<EditItemModel> logger)
        {
            _itemsService = itemsService;
            _categoriesService = categoriesService;
            _retailersService = retailersService;
            _logger = logger;
        }


        private string _itemId;

        [FromRoute]
        public string ItemId 
        { 
            get
            {
                return _itemId;
            }
            set
            {
                _itemId = Uri.UnescapeDataString(value);
            } 
        }

        public ItemViewModel Item { get; set; }

        [BindProperty]
        public UpdateItemViewModel UpdatedItem { get; set; }

        public CategoriesViewModel AvailableCategories { get; set; }

        public RetailersViewModel AvailableRetailers { get; set; }

        public WebApiCallException Error { get; set; }


        public async Task<IActionResult> OnGet()
        {
            var tasks = new List<Task>();

            tasks.Add(Task.Run(async () => Item = await _itemsService.GetItemByIdAsync(ItemId)));
            tasks.Add(Task.Run(async () => AvailableRetailers = await _retailersService.GetAllRetailersAsync(Int32.MaxValue, 1)));
            tasks.Add(Task.Run(async () => AvailableCategories = await _categoriesService.GetAllCategoriesAsync(Int32.MaxValue, 1)));

            await Task.WhenAll(tasks);

            return null;
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return await OnGet();
                }

                await _itemsService.UpdateItemAsync(ItemId, UpdatedItem);

                return RedirectToPage("/Items/Items");
            }
            catch (WebApiCallException wace)
            {
                Error = wace; // set error on model
                return await OnGet();
            }
        }

        public async Task<IActionResult> OnPostDeleteImage()
        {
            await _itemsService.UpdateItemPartialAsync(ItemId);
            
            return new JsonResult(null) { StatusCode = (int)HttpStatusCode.OK };
        }
    }
}
