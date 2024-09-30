using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MoviesAPI.Models.System
{
    public class LoginResponse
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("korisnik")]
        public User Korisnik { get; set; }

       
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
