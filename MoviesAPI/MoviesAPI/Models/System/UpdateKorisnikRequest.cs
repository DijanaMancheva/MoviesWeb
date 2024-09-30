using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CodexIntraAPI.Models.System
{
    public class UpdateKorisnikRequest
    {
        [Required]
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [Required]
        [JsonPropertyName("pass")]
        public string Pass { get; set; }

        [Required]
        [JsonPropertyName("tp")]
        public int TP { get; set; }

        [Required]
        [JsonPropertyName("p")]
        public int P { get; set; }

        [Required]
        [JsonPropertyName("re")]
        public int Re { get; set; }

        [JsonPropertyName("aktiven")]
        public short Aktiven { get; set; }

        [JsonPropertyName("administrator")]
        public short Administrator { get; set; }
    
        [JsonPropertyName("administrator_bool")]
        public bool? Administrator_bool
        {
            set
            {
                Administrator = value == true ? (short)1 : (short)0;
            }
            get
            {
                if (Administrator == 1)
                    return true;
                else
                    return false;
            }
        }

        [JsonPropertyName("aktiven_bool")]
        public bool? Aktiven_bool
        {
            set
            {
                Aktiven = value == true ? (short)1 : (short)0;
            }
            get
            {
                if (Aktiven == 1)
                    return true;
                else
                    return false;
            }
        }
    }
}
