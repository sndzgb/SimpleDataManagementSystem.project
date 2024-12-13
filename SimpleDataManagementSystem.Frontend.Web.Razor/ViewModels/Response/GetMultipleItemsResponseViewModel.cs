using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response
{
    public sealed class GetMultipleItemsResponseViewModel
	{
		[JsonPropertyName("items")]
		public List<ItemViewModel>? Items { get; set; }
		
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

		public class ItemViewModel
		{
			[JsonPropertyName("nazivproizvoda")]
			public string Nazivproizvoda { get; set; }
			
			[JsonPropertyName("opis")]
			public string Opis { get; set; }
			
			[JsonPropertyName("datumakcije")]
			public string Datumakcije { get; set; }

			[JsonPropertyName("URLdoslike")]
			public string URLdoslike { get; set; }
			
			[JsonPropertyName("cijena")]
			public decimal Cijena { get; set; }
			
			[JsonPropertyName("isEnabled")]
			public bool IsEnabled { get; set; }

			[JsonPropertyName("monitoring")]
			public MonitoredItemViewModel Monitoring { get; set; }

			public class MonitoredItemViewModel
			{
				[JsonPropertyName("isMonitoredByCurrentUser")]
				public bool IsMonitoredByCurrentUser { get; set; }
				
				[JsonPropertyName("totalUsersMonitoringThisItem")]
				public int TotalUsersMonitoringThisItem { get; set; }
			}

			[JsonPropertyName("retailer")]
			public RetailerViewModel? Retailer { get; set; }
			
			[JsonPropertyName("category")]
			public CategoryViewModel? Category { get; set; }

			public class RetailerViewModel
			{
				[JsonPropertyName("id")]
				public int Id { get; set; }
				
				[JsonPropertyName("name")]
				public string Name { get; set; }
				
				[JsonPropertyName("priority")]
				public int Priority { get; set; }
			}

			public class CategoryViewModel
			{
				[JsonPropertyName("id")]
				public int Id { get; set; }
				
				[JsonPropertyName("name")]
				public string Name { get; set; }
			}
		}
	}
}
