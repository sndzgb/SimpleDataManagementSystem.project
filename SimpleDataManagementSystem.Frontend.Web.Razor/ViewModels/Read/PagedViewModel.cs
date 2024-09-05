using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read
{
    public class PagedViewModel
    {
        public PagedViewModel(int total, int page = 1, int take = 8)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (take < 1)
            {
                take = 8;
            }

            var totalPages = (int)Math.Ceiling((decimal)total / (decimal)take);
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;

            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            Total = total;
            Page = currentPage;
            Take = take;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }


        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }
        
        [JsonPropertyName("take")]
        public int Take { get; set; }

        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
    }
}
