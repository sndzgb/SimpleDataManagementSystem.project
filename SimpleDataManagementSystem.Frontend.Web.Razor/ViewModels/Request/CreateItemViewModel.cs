using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Shared.Web.Validators;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request
{
    public class CreateItemRequestViewModel
    {
        //[Required(ErrorMessage = "Nazivproizvoda is required")]
        //[StringLength(maximumLength: 255, ErrorMessage = "Invalid 'Nazivproizvoda' length", MinimumLength = 2)]
        public string? Nazivproizvoda { get; set; }

        [StringLength(maximumLength: 255, ErrorMessage = "Invalid 'Opis' length")]
        public string? Opis { get; set; }

        [StringLength(maximumLength: 255, ErrorMessage = "Invalid 'Datumakcije' length", MinimumLength = 2)]
        public string? Datumakcije { get; set; }

        [Required(ErrorMessage = "Retailer is required")]
        public int? RetailerId { get; set; }

        [Required(ErrorMessage = "Cijena is required")]
        [DecimalValidator]
        public string Cijena { get; set; }

        [Required(ErrorMessage = "Kategorija is required")]
        [Range(int.MinValue, int.MaxValue, ErrorMessage = "Invalid 'Kagegorija' value")]
        public int? Kategorija { get; set; }

        [IgnoreDataMember]
        [MaxFileSizeValidator(8 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is {0} bytes")]
        [AllowedExtensionsValidator(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? URLdoslike { get; set; }

        public bool IsEnabled { get; set; }
    }

    public class CreateItemResponseViewModel
    {
        [JsonPropertyName("nazivproizvoda")]
        public string Nazivproizvoda { get; set; }

        [JsonPropertyName("opis")]
        public string? Opis { get; set; }

        [JsonPropertyName("datumakcije")]
        public string? DatumAkcije { get; set; }

        [JsonPropertyName("URLdoslike")]
        public string? URLdoslike { get; set; }

        [JsonPropertyName("cijena")]
        public decimal Cijena { get; set; }

        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonPropertyName("category")]
        public CategoryViewModel Category { get; set; }

        [JsonPropertyName("retailer")]
        public RetailerViewModel Retailer { get; set; }

        public class CategoryViewModel
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("priority")]
            public int Priority { get; set; }
        }

        public class RetailerViewModel
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("priority")]
            public int Priority { get; set; }

            [JsonPropertyName("logoImageUrl")]
            public string? LogoImageUrl { get; set; }
        }
    }
}
