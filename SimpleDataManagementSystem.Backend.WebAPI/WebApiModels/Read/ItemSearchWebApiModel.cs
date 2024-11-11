using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read
{
    public class ItemSearchResponseWebApiModel
    {
        public ItemSearchResponseWebApiModel()
        {
            this.Items = new List<ItemWebApiModel>();
            this.Request = new ItemSearchRequestWebApiModel();
            this.PageInfo = new PagedWebApiModel();
        }

        public List<ItemWebApiModel> Items { get; set; }
        public ItemSearchRequestWebApiModel Request { get; set; }
        public PagedWebApiModel PageInfo { get; set; }
    }

    public class ItemSearchRequestWebApiModel
    {
        public string? SearchQuery { get; set; }
        public int Page { get; set; }
        public int Take { get; set; }
        public SearchableItemSortOrder SortBy { get; set; }
    }
}
