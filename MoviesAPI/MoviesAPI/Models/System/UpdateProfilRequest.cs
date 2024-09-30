using System.Text.Json.Serialization;

namespace CodexIntraAPI.Models.System
{
    public class UpdateProfilRequest
    {
        [JsonPropertyName("ime")]
        public string Ime { get; set; }

        [JsonPropertyName("prezime")]
        public string Prezime { get; set; }

        [JsonPropertyName("pass")]
        public string Pass { get; set; }      
    }
}
