
using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Dapper;
    using Microsoft.Extensions.Options;
    using MoviesAPI.Models;
    using Newtonsoft.Json;
    using Npgsql;
namespace MoviesAPI.Repositories
{
    

    public class ReviewRepository : IReviewRepository
    {
        private readonly MoviesDB _dbSettings;

        //private readonly HttpClient _httpClient;

        public ReviewRepository(IOptions<MoviesDB> dbSettings)
        {
            _dbSettings = dbSettings.Value;

        }
        public async Task<IEnumerable<Review>> GetReviewMovieAsync()
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            String sql = @"select
                           R.id,
                           R.id_movie,
                            R.id_user,
                            R.content,
                            R.createdAt,
                            R.Rating,
                            M.title,
                            U.name
                        from public.review R
                        join public.movie M on M.id=R.id_movie
                        join public.user U on U.id=R.id_user
                        ";
            var result = await conn.QueryAsync<Review>(sql);
            return result;
        }
        public async Task<IEnumerable<Review>> GetReviewShowAsync()
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            String sql = @"select
                           R.id,
                           R.id_show,
                            R.id_user,
                            R.content,
                            R.createdAt,
                            R.Rating
                        from public.review R";
            var result = await conn.QueryAsync<Review>(sql);
            return result;
        }
        public async Task<Review> GetReviewMovieAsync(int id)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            String sql = @"select
                           R.id,
                           R.id_movie,
                            R.id_user,
                            R.content,
                            R.createdAt,
                            R.Rating,
                            M.title,
                            U.name
                        from public.review R
                        join public.movie M on M.id=R.id_movie
                        join public.user U on U.id=R.id_user
                        where R.id=@Id";
            var result = await conn.QueryAsync<Review>(sql, new { id });
            return result.FirstOrDefault();

        }
        public async Task<Review> GetReviewShowAsync(int id)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            String sql = @"select
                           R.id,
                           R.id_show,
                            R.id_user,
                            R.content,
                            R.createdAt,
                            R.Rating
                        from public.review R
                        where R.id=@Id";
            var result = await conn.QueryAsync<Review>(sql, new { id });
            return result.FirstOrDefault();

        }
        public async Task<IEnumerable<Review>> GetReviewForMovieAsync(int id_movie)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            { 
                String sql = @"select
                           R.id,
                           R.id_movie,
                            R.id_user,
                            R.content,
                            R.createdAt,
                            R.Rating,
                            M.title,
                            U.name
                        from public.review R
                        join public.movie M on M.id=R.id_movie
                        join public.user U on U.id=R.id_user
                        where R.id_movie=@Id_movie";
            return await conn.QueryAsync<Review>(sql, new { Id_movie = id_movie });

            }
           
            

        }
        public async Task<IEnumerable<Review>> GetReviewForMoviesAsync(int id)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            {
                String sql = @"select
                           R.id,
                           R.id_movie,
                            R.id_user,
                            R.content,
                            R.createdAt,
                            R.Rating,
                            M.title,
                            U.name
                        from public.review R
                        join public.movie M on M.id=R.id_movie
                        join public.user U on U.id=R.id_user
                        where R.id_movie=@Id_movie";
                return await conn.QueryAsync<Review>(sql, new { Id = id });

            }
        }
            public async Task<IEnumerable<Review>> GetReviewForShowAsync(int id_show)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            {
                String sql = @"select
                           R.id,
                           R.id_show,
                            R.id_user,
                            R.content,
                            R.createdAt,
                            R.Rating,
                            M.title,
                            U.name
                        from public.review R
                        join public.show M on M.id=R.id_show
                        join public.user U on U.id=R.id_user
                        where R.id_show=@Id_show";
                return await conn.QueryAsync<Review>(sql, new { Id_show = id_show });

            }



        }
        public async Task<IEnumerable<Review>> GetReviewForShowsAsync(int id)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            {
                String sql = @"select
                           R.id,
                           R.id_show,
                            R.id_user,
                            R.content,
                            R.createdAt,
                            R.Rating,
                            M.title,
                            U.name
                        from public.review R
                        join public.show M on M.id=R.id_show
                        join public.user U on U.id=R.id_user
                        where R.id_show=@Id_show";
                return await conn.QueryAsync<Review>(sql, new { Id = id});

            }



        }
        public async Task<UpdateReview> GetMovieReviewForUpdateAsync(int id)
        {
              using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"select
                           R.id,
                           R.id_movie,
                            R.id_user,
                            R.content,
                            R.createdAt,
                            R.Rating,
                            M.title,
                            U.name
                        from public.review R
                        join public.movie M on M.id=R.id_movie
                        join public.user U on U.id=R.id_user
                        where R.id=@Id";
                var result = await conn.QueryAsync<UpdateReview>(sql, new { Id=id });
                return result.FirstOrDefault();
            }

        }
        public async Task<UpdateReview> GetShowReviewForUpdateAsync(int id)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"select
                           R.id,
                           R.id_show,
                            R.id_user,
                            R.content,
                            R.createdAt,
                            R.Rating,
                            M.title,
                            U.name
                        from public.review R
                        join public.show M on M.id=R.id_show
                        join public.user U on U.id=R.id_user
                        where R.id=@Id";
                var result = await conn.QueryAsync<UpdateReview>(sql, new { Id = id });
                return result.FirstOrDefault();
            }

        }
        public async Task<int> CreateMovieReviewAsync(CreateReview newReview)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"INSERT INTO public.review(id_user,id_movie,content,createdAt,rating)
                            values(@Id_user,@Id_movie,@Content,@CreatedAt,@Rating)
                            returning id";
                return await conn.ExecuteScalarAsync<int>(sql,newReview);
            }

        }
        public async Task<int> CreateShowReviewAsync(CreateReview newReview)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"INSERT INTO public.review(id_user,id_show,content,createdAt,rating)
                            values(@Id_user,@Id_show,@Content,@CreatedAt,@Rating)
                            returning id";
                return await conn.ExecuteScalarAsync<int>(sql, newReview);
            }

        }
        public async Task<int> UpdateMovieReviewAsync(int id,UpdateReview newReview)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                
                String sql = @"UPDATE public.review
                                SET
                                    id=@Id,
                                    id_user=@Id_user,
                                    id_movie=@Id_movie,
                                    content=@Content,
                                    createdAt=@CreatedAt,
                                    rating=@Rating
                                WHERE id=@Id";
                return await conn.ExecuteAsync(sql, new { newReview.Id, newReview.Id_user, newReview.Id_movie, newReview.Content, newReview.CreatedAt ,newReview.Rating});
            }
        }
        public async Task<int> UpdateShowReviewAsync(int id, UpdateReview newReview)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"UPDATE public.review
                                SET
                                    id=@Id,
                                    id_user=@Id_user,
                                    id_show=@Id_show,
                                    content=@Content,
                                    createdAt=@CreatedAt,
                                    rating=@Rating
                                WHERE id=@Id";
                return await conn.ExecuteAsync(sql, new { newReview.Id, newReview.Id_user, newReview.Id_show, newReview.Content, newReview.CreatedAt, newReview.Rating });
            }
        }
        public async Task<int> DeleteMovieReviewAsync(int id)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"DELETE FROM public.review R
                           WHERE R.id=@Id";
                return await conn.ExecuteAsync(sql, new { Id=id });
            }

        }
        public async Task<int> DeleteShowReviewAsync(int id)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"DELETE FROM public.review R
                           WHERE R.id=@Id";
                return await conn.ExecuteAsync(sql, new { Id = id });
            }

        }

    }

}
