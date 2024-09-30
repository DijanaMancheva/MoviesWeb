using CodexIntraAPI.Authentication;
using CodexIntraAPI.Models.System;
using Dapper;
using Microsoft.Extensions.Options;
using MoviesAPI.Models;
using MoviesAPI.Models.System;
using Npgsql;

namespace MoviesAPI.Authentication
{
    public class KorisnikService : IKorisnikService
    {
        private readonly ILogger<KorisnikService> _logger;
        private readonly MoviesDB _dbSettings;

        public KorisnikService(ILogger<KorisnikService> logger, IOptions<MoviesDB> dbSettings)
        {
            _logger = logger;
            _dbSettings = dbSettings.Value;
            //_klientService = klientService;
        }

        public User ProveriKorisnikAkreditivi(string username, string password)
        {
            _logger.LogInformation($"Проверка на логирање за корисник: [{username}]");
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            return Authenticate(username, password);
        }

        public async Task<IEnumerable<User>> ZemiKorisniciAsync()
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                conn.Open();
                String sql = @"select TRIM(U.USERNAME) as USERNAME,
                                U.ACTIVE,
                               U.PASSWORD,
                                U.PHONE,
                                U.NAME,
                                P.ID
                                
                        from USER U";
                var result = await conn.QueryAsync<User>(sql);

                return result;
            }
           
            
        }

       
        
     
        /// <summary>
        /// Автентикација на внесените податоци за корисник и лозинка во базата
        /// </summary>
        /// <param name="username">Внесени податоци за корисникот</param>
        /// <param name="password">Внесени податоци за корисникот</param>
        /// <returns></returns>
        private User Authenticate(string username, string password)
        {
            User korisnik = ZemiKorisnik(username);

            // ако има таков корисник
            if (korisnik is not null)
            {
                // Провери дали корисникот е активен
                if (korisnik.Active == true)
                {
                   

                    // Проверка на совпаѓање на лозинката во базата со ново хашираната внесена лозинка
                    if (password == korisnik.Password)
                    {
                        // Врати податоци за успешно логираниот корисник
                        return korisnik;
                    }
                }

            }
            return null;
        }

        public bool ProveriPostoeckiKorisnik(string username)
        {
            User korisnik = ZemiKorisnik(username);

            return korisnik is not null;
        }

        public UpdateKorisnikRequest ZemiKorisnikZaUpdate(string username)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                conn.Open();

                string sql = @"select  TRIM(U.USERNAME) as USERNAME,
                                U.ACTIVE,
                               U.PASSWORD,
                                U.PHONE,
                                U.NAME,
                                P.ID
                                
                        from USER U";

                UpdateKorisnikRequest korisnik = conn.Query<UpdateKorisnikRequest>(sql, new { username }).FirstOrDefault();

                return korisnik;
            }
        }

        public User ZemiKorisnik(string username)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                

                string sql = @"select 
                                U.active,
                               U.password,
                                TRIM(U.username) as username,
                                U.phone,
                                U.name,
                               U.id
                                
                        from public.user U
                                where TRIM(UPPER(U.username)) = TRIM(UPPER(@username))";

                User korisnik = conn.Query<User>(sql, new { username }).FirstOrDefault();

                return korisnik;
            }
        }

        public void KreirajKorisnik(CreateUser request)
        {
            // Генерирај уникатниот salt за корисникот
            byte[] new_salt = Security.GenerateSalt();
            // Претвори го salt-от во стринг за да се зачува во базата
            string salt = Convert.ToBase64String(new_salt);
            // Внесената лозинка хаширај ја со додаден salt
            string pass = Security.HashPassword(request.Password, new_salt);

            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                string sql = @"insert into public.user (password,username,phone,name)
                               values (@Password,TRIM(UPPER(@Username)),@Phone,@Name)";

                

                conn.Execute(sql, new {  request.Password, request.Username, request.Phone, request.Name });

               
                //var aktiven = request.Aktiven_bool == true ? 1 : 0;

            }
        }

        public void AzurirajKorisnik(string username, UpdateKorisnikRequest request)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                string sql = @"UPDATE public.user
                        SET active=@Active,
                            password=@Password,
                            username=@Username,
                            phone=@Phone,
                            name=@Name,
                            id=@Id
                        WHERE username=@Username";

                conn.Execute(sql, new { request.Username });

              
            }
        }

        
        public int BrisiKorisnik(string username)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"DELETE FROM public.user U
                           WHERE U.username=@Username";

                return conn.Execute(sql, new { username });
            }
        }

        
    }
}
