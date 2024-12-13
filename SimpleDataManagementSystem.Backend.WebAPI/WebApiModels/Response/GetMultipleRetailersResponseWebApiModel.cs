namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Response
{
    public sealed class GetMultipleRetailersResponseWebApiModel
    {
        public List<RetailerWebApiModel> Retailers { get; set; }
        public PageWebApiModel PageInfo { get; set; }

        public class RetailerWebApiModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string LogoImageUrl { get; set; }
            public int Priority { get; set; }
        }

        public class PageWebApiModel
        {
            public int Total { get; set; }
            public int Page { get; set; }
            public int Take { get; set; }
        }
    }
}
