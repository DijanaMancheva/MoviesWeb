using MoviesWeb.Models.System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MoviesWeb.Models;
using MoviesWeb.Services;


namespace MoviesWeb.Services
{
    public class UserService : IUserService
    {
        private readonly CxSettings _cxSettings;
        public const string IntraApiToken = "IntraApiToken";
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserService(IOptions<CxSettings> cxSettings, IHttpContextAccessor httpContextAccessor)
        {
            _cxSettings = cxSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            
        }

        public async Task<LoginResponse> ProveriKorisnikAkreditivi(LoginRequest loginRequest)
        {
            LoginResponse loginResponse = new LoginResponse();

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:44367/api");

                HttpContent requestContent = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(httpClient.BaseAddress + "/User/Login", requestContent))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        loginResponse = JsonConvert.DeserializeObject<LoginResponse>(apiResponse);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return loginResponse;
        }
        

        //public async Task<bool> AzurirajKorisnik(UpdateProfilRequest updateRequest)
        //{


        //}
    }
}
