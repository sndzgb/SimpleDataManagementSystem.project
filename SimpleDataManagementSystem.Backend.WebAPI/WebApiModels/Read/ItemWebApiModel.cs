using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read
{
    public class ItemWebApiModel
    {
        public string Nazivproizvoda { get; set; }
        public string Opis { get; set; }
        public string Datumakcije { get; set; }
        public string Nazivretailera { get; set; }

        [JsonPropertyName("URLdoslikeUri")]
        public string URLdoslikeUri { get; set; }
        public decimal Cijena { get; set; }
        public int Kategorija { get; set; }
    }
}
