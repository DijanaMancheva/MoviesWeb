using System.Collections.Generic;
using System.Threading.Tasks;
using CodexIntraAPI.Models.System;
using MoviesAPI.Models;

namespace CodexIntraAPI.Authentication
{
    public interface IKorisnikService
    {
        Task<IEnumerable<User>> ZemiKorisniciAsync();

        User ProveriKorisnikAkreditivi(string username, string pass);

        User ZemiKorisnik(string username);



        UpdateKorisnikRequest ZemiKorisnikZaUpdate(string username);

        bool ProveriPostoeckiKorisnik(string username);

        void KreirajKorisnik(CreateUser request);

        void AzurirajKorisnik(string username, UpdateKorisnikRequest request);

        

        int BrisiKorisnik(string username);

        
    }
}
