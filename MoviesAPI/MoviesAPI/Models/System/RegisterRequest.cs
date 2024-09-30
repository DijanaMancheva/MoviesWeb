using System.Text.Json.Serialization;

namespace MoviesAPI.Models.System
{
    public class RegisterRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("pass")]
        public string Password { get; set; }

       
      

  

    }
}
