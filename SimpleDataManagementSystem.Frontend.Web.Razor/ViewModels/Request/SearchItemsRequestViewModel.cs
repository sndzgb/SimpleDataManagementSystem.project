using SimpleDataManagementSystem.Shared.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request
{
    public class SearchItemsRequestViewModel
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

        [JsonPropertyName("query")]
        [Required(ErrorMessage = "Search query is required.")]
        public string Query { get; set; }

        [JsonPropertyName("page")]
        public int? Page { get; set; } = 1;

        [JsonPropertyName("take")]
        public int? Take { get; set; } = 8;

        [JsonPropertyName("enabledOnly")]
        public bool EnabledOnly { get; set; } = true;
    }
}
