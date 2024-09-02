using System.Globalization;
using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read
{
    public class ItemViewModel
    {
        [JsonPropertyName("nazivproizvoda")]
        public string Nazivproizvoda { get; set; }

        [JsonPropertyName("opis")]
        public string Opis { get; set; }

        [JsonPropertyName("datumakcije")]
        public string Datumakcije { get; set; }

        [JsonPropertyName("nazivretailera")]
        public string Nazivretailera { get; set; }

        [JsonPropertyName("URLdoslikeUri")]
        public string URLdoslikeUri { get; set; }

        private decimal _cijena;

        [JsonPropertyName("cijena")]
        public decimal Cijena 
        {
            get 
            {
                return _cijena; // = decimal.Parse(_cijena.ToString(), CultureInfo.InvariantCulture); 
            }
            set 
            { 
                _cijena = value; 
            }
        }

        [JsonPropertyName("kategorija")]
        public int Kategorija { get; set; }
    }
}
