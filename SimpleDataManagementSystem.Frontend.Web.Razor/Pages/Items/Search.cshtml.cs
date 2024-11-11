using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Items
{
    public class SearchModel : BasePageModel<ItemsSearchResponseViewModel>
    {
        private readonly IItemsService _itemsService;
        private readonly IAuthorizationService _authorizationService;


        [FromQuery(Name = "searchQuery")]
        public string SearchQuery { get; set; }

        private int? _itemsPerPage = 8;
        [FromQuery(Name = "take")]
        public int? ItemsPerPage 
        { 
            get
            {
                return _itemsPerPage ?? 8;
            }
            set
            {
                if (value == null)
                {
                    _itemsPerPage = 8;
                    return;
                }
                _itemsPerPage = value;
            }
        }

        private int? _pageNumber = 1;
        [FromQuery(Name = "page")]
        public int? PageNumber
        {
            get
            {
                return _pageNumber ?? 1;
            }
            set
            {
                if (value == null)
                {
                    _pageNumber = 1;
                    return;
                }
                _pageNumber = value;
            }
        }

        [FromQuery(Name = "sortBy")]
        public int? SortBy { get; set; } = (int)SortableItem.NazivproizvodaDesc;


        public SearchModel(IItemsService itemsService, IAuthorizationService authorizationService)
        {
            _itemsService = itemsService;
            _authorizationService = authorizationService;
        }


        public override async void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                HttpContext.User,
                new { roles },
                Policies.PolicyNames.UserIsInRole
            );

            if (!authorizationResult.Succeeded)
            {
                context.Result = Forbid();
                return;
            }

            base.OnPageHandlerExecuting(context);
        }

        public async Task<IActionResult> OnGet()
        {
            Model = await _itemsService.SearchItemsAsync(
                new ItemsSearchRequestViewModel()
                {
                    SearchQuery = SearchQuery,
                    Page = PageNumber,
                    SortBy = SortBy,
                    Take = ItemsPerPage
                }
            );

            return Page();
        }
    }
}
