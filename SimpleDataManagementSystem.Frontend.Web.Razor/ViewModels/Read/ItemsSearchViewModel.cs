using Microsoft.AspNetCore.Razor.TagHelpers;
using SimpleDataManagementSystem.Shared.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read
{
    public class ItemsSearchRequestViewModel
    {
        private int? _sortBy = (int)SortableItem.NazivproizvodaDesc;

        [JsonPropertyName("sortBy")]
        public int? SortBy 
        { 
            get
            {
                return _sortBy;
            }
            set
            {
                if (EnumHelpers.IsDefined<SortableItem>((int)value!))
                {
                    _sortBy = value;
                }
                else
                {
                    _sortBy = (int)SortableItem.NazivproizvodaDesc;
                }
            } 
        }

        [JsonPropertyName("searchQuery")]
        [Required(ErrorMessage = "Search query is required.")]
        public string SearchQuery { get; set; } = string.Empty;

        [JsonPropertyName("page")]
        public int? Page { get; set; } = 1;

        [JsonPropertyName("take")]
        public int? Take { get; set; } = 8;
    }

    public class ItemsSearchResponseViewModel
    {
        [JsonPropertyName("items")]
        public List<ItemsSearchResult> Items { get; set; }

        [JsonPropertyName("request")]
        public ItemsSearchRequestViewModel Request { get; set; }

        [JsonPropertyName("pageInfo")]
        public PagedViewModel PageInfo { get; set; }
    }

    public class ItemsSearchResult
    {
        [JsonPropertyName("nazivproizvoda")]
        public string Nazivproizvoda { get; set; }

        [JsonPropertyName("cijena")]
        public decimal Cijena { get; set; }

        [JsonPropertyName("URLdoslikeUri")]
        public string? URLdoslikeUri { get; set; }
    }

    public enum SortableItem
    {
        [Description("Naziv proizvoda asc")]
        NazivproizvodaAsc = 10,
        [Description("Naziv proizvoda desc")]
        NazivproizvodaDesc = 11,
        [Description("Cijena asc")]
        CijenaAsc = 20,
        [Description("Cijena desc")]
        CijenaDesc = 21
    }
}
