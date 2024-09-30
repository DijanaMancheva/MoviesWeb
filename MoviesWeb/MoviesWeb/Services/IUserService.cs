using MoviesWeb.Models.System;
using Microsoft.AspNetCore.Http;
using MoviesWeb.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesWeb.Services
{
    public interface IUserService
    {
        Task<LoginResponse> ProveriKorisnikAkreditivi(LoginRequest loginRequest);
        //Task<bool> AzurirajKorisnik(UpdateProfilRequest updateRequest);

    }
}
