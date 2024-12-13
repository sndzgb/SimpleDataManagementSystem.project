using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response
{
    public sealed class GetMultipleCategoriesResponseViewModel
	{
		[JsonPropertyName("categories")]
		public List<CategoryViewModel>? Categories { get; set; }
		
		[JsonPropertyName("pageInfo")]
		public PagedViewModel PageInfo { get; set; }

		//public class PageViewModel
		//{
		//	[JsonPropertyName("total")]
		//	public int Total { get; set; }
			
		//	[JsonPropertyName("page")]
		//	public int Page { get; set; }
			
		//	[JsonPropertyName("take")]
		//	public int Take { get; set; }
		//}

		public class CategoryViewModel
		{
			[JsonPropertyName("id")]
			public int Id { get; set; }
			
			[JsonPropertyName("name")]
			public string Name { get; set; }
			
			[JsonPropertyName("priority")]
			public int Priority { get; set; }
		}
	}
}
