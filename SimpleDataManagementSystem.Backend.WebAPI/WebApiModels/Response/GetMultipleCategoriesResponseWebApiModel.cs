namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Response
{
    public sealed class GetMultipleCategoriesResponseWebApiModel
    {
        public List<CategoryWebApiModel>? Categories { get; set; }
        public PageWebApiModel PageInfo { get; set; }

        public class PageWebApiModel
        {
            public int Total { get; set; }
            public int Page { get; set; }
            public int Take { get; set; }
        }

        public class CategoryWebApiModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Priority { get; set; }
        }
    }
}
