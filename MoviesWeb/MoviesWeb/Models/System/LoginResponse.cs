using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesWeb.Models.System
{
    public class LoginResponse

    {
        public User User { get; set; }

        [JsonProperty("accessToken")]
        public string Token { get; set; }

        [JsonProperty("korisnik")]
        public User Korisnik { get; set; }

       

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
