using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Read
{
    public class ItemSearchResponseDTO
    {
        public ItemSearchResponseDTO()
        {
            this.Items = new List<ItemDTO>();
            this.Request = new ItemSearchRequestDTO();
            this.PageInfo = new PagedDTO();
        }

        public List<ItemDTO> Items { get; set; }
        public ItemSearchRequestDTO Request { get; set; }
        public PagedDTO PageInfo { get; set; }
    }

    public class ItemSearchRequestDTO
    {
        public string? SearchQuery { get; set; }
        public int Page { get; set; }
        public int Take { get; set; }
        public SearchableItemSortOrder SortBy { get; set; }
    }

    public enum SearchableItemSortOrder
    {
        NazivproizvodaAsc = 10,
        NazivproizvodaDesc = 11,
        CijenaAsc = 20,
        CijenaDesc = 21
    }
}
