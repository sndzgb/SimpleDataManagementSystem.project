using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response
{
    public sealed class GetMultipleRetailersResponseViewModel
	{
		[JsonPropertyName("retailers")]
		public List<RetailerViewModel> Retailers { get; set; }
		
		[JsonPropertyName("pageInfo")]
		public PagedViewModel PageInfo { get; set; }

		public class RetailerViewModel
		{
			[JsonPropertyName("id")]
			public int Id { get; set; }
			
			[JsonPropertyName("name")]
			public string Name { get; set; }

			[JsonPropertyName("logoImageUrl")]
			public string LogoImageUrl { get; set; }
			
			[JsonPropertyName("priority")]
			public int Priority { get; set; }
		}

		//public class PageViewModel
		//{
		//	[JsonPropertyName("total")]
		//	public int Total { get; set; }
			
		//	[JsonPropertyName("page")]
		//	public int Page { get; set; }
			
		//	[JsonPropertyName("take")]
		//	public int Take { get; set; }
		//}
	}
}
