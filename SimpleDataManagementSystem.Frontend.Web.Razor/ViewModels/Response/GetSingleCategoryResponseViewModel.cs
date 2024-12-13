using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response
{
	public sealed class GetSingleCategoryResponseViewModel
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		
		[JsonPropertyName("name")]
		public string Name { get; set; }
		
		[JsonPropertyName("priority")]
		public int Priority { get; set; }
	}
}
