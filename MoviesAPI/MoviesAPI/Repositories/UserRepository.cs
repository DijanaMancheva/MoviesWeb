using System.Security.Claims;
using Dapper;
using Microsoft.Extensions.Options;
using MoviesAPI.Models;
using Npgsql;
using static System.Net.WebRequestMethods;

namespace MoviesAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MoviesDB _dbSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserRepository(IOptions<MoviesDB> dbSettings, IHttpContextAccessor httpContextAccessor)
        {
            _dbSettings = dbSettings.Value;
            _httpContextAccessor=httpContextAccessor;
        }

        public async Task<IEnumerable<User>> GetUserAsync()
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            String sql = @"select
                            U.ACTIVE,
                            U.PASSWORD,
                            U.USERNAME,
                            U.PHONE,
                            U.NAME,
                            U.ID,
                            U.ADMIN,
                            U.EMAIL
                        from public.user U";
            var result = await conn.QueryAsync<User>(sql);
            return result;
        }

        public async Task<User> GetUserAsync(int id)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            {
                String sql = @"select
                            U.id,
                            U.active,
                            U.password,
                            U.username,
                            U.phone,
                            U.name,
                            U.admin,
                            U.email
                        from public.user U
                        where U.id=@Id";
                var result = await conn.QueryAsync<User>(sql, new { id });
                return result.FirstOrDefault();
            }
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            {
                String sql = @"select
                            U.active,
                            U.password,
                            U.username,
                            U.phone,
                            U.name,
                            U.admin,
                            U.email,
                            U.id
                        from public.user U
                        where U.email=@Email";
                var result = await conn.QueryAsync<User>(sql, new { email });
                return result.FirstOrDefault();
            }
        }
        //public async Task<User> GetUserByPasswordAsync(string password)
        //{
        //    using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
        //    {
        //        String sql = @"select
        //                    U.active,
        //                    U.password,
        //                    U.username,
        //                    U.phone,
        //                    U.name,
        //                    U.admin,
        //                    U.email,
        //                    U.id
        //                from public.user U
        //                where U.password=@Password";
        //        var result = await conn.QueryAsync<User>(sql, new { password });
        //        return result.FirstOrDefault();
        //    }
        //}
        public async Task<User> GetUserForUpdateAsync(int id)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"select 
                                    U.active,
                                    U.password,
                                    U.username,
                                    U.phone,
                                    U.name,
                                       U.id,
                                    U.admin,
                            U.email

                            from public.user U
                            where U.id=@Id";
                var result = await conn.QueryAsync<User>(sql, new { id });
                return result.FirstOrDefault();
            }
        }

        public async Task<int> CreateUserAsync(CreateUser newUser)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"INSERT INTO public.user(active ,password ,username ,phone ,name,admin,email)
                        values(@Active ,@Password,@Username ,@Phone ,@Name ,@Admin,@Email)
                        returning id";
                return await conn.ExecuteScalarAsync<int>(sql, newUser);
            }
        }

        public async Task<int> UpdateUserAsync(int id, User newUser)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"UPDATE public.user
                        SET active=@Active,
                            password=@Password,
                            username=@Username,
                            phone=@Phone,
                            name=@Name,
                            id=@Id,
                            admin=@Admin,
                            email=@Email
                        WHERE id=@Id";
                return await conn.ExecuteAsync(sql, new { newUser.Id, newUser.Active,newUser.Password,newUser.Username,newUser.Phone,newUser.Name,newUser.Admin,newUser.Email });
            }
        }
        public async Task<int> UpdatePasswordAsync(int id, User newUser)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"UPDATE public.user
                        SET 
                            password=@Password,
                           id=@Id
                        WHERE id=@Id";
                return await conn.ExecuteAsync(sql, new { newUser.Id,  newUser.Password });
            }
        }


        public async Task<int> DeleteUserAsync(int id)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"DELETE FROM public.user U
                           WHERE U.id=@Id";
                return await conn.ExecuteAsync(sql, new { id });
            }

        }
        public IEnumerable<Claim> GetUserClaimsAsync()
        {
            return _httpContextAccessor.HttpContext.User.Claims;
        }
        public async Task<long> GetDashInfoAsync()
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                string sql = @"select total_users();";

                var result = await conn.ExecuteScalarAsync<long>(sql);

                return result;
            }

        }

    }
}

