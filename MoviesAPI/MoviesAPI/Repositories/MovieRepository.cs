using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MoviesAPI.Models;
using Npgsql;

namespace MoviesAPI.Repositories
{
    public class MovieRepository : IMovieRepository
    {

        private readonly MoviesDB _dbSettings;
        private readonly IMovieRepository _movieRepository;
        public MovieRepository(IOptions<MoviesDB> dbSettings)
        {
            _dbSettings = dbSettings.Value;

        }
        //public MovieController(IMovieRepository movieRepository)
        //{
        //    _movieRepository = movieRepository;
        //}
        //public async Task<IActionResult> Index(string searchString)
        //{
        //    var movies = await _movieRepository.GetMovieAsync();
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        movies=movies.Where(n=>n.Title.Contains(searchString) || n.Genre.Contains(searchString) || n.Director.Contains(searchString)).ToList();
        //    }
        //    return View(movies);
        //}
        public async Task<IEnumerable<Movie>> GetMovieAsync()
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            String sql = @"select
                            M.AMOUNT,
                            M.RELEASE_DATE,
                            M.DURATION,
                            M.TITLE,
                            M.ID,
                            M.GENRE,
                            M.DIRECTOR,
                            M.DESCRIPTION,
                            M.POSTER_IMAGE,
                            M.PRICE,
                            M.ISAVAILABLE,
                            M.TRAILER

                        from public.movie M";
            var result = await conn.QueryAsync<Movie>(sql);
            return result;
        }
        public async Task<IEnumerable<Movie>> GetMovieDevAsync()
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            String sql = @"select
                            M.AMOUNT,
                            M.RELEASE_DATE,
                            M.DURATION,
                            M.TITLE,
                            M.ID,
                           M.TYPE,
                            M.DIRECTOR,
                            M.DESCRIPTION,
                            M.POSTER_IMAGE,
                            M.PRICE,
                            M.ISAVAILABLE,
                            M.TRAILER

                        from public.movie M join public.zanrovi Z on M.type=Z.type
                        ";

            var result = await conn.QueryAsync<Movie>(sql);
            return result;
        }
        public async Task<Movie> GetMovieDevAsync(int id)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            {
                String sql = @"select
                            M.id,
                            M.amount,
                            M.release_date,
                            M.duration,
                            M.title,
                            M.type,
                            M.director,
                            M.description,
                            M.poster_image,
                            M.price,
                            M.isavailable,
                            M.trailer
                        from public.movie M join public.zanrovi Z on M.type=Z.type
                        where M.id=@Id";
                var result = await conn.QueryAsync<Movie>(sql, new { id });

                return result.FirstOrDefault();
            }
        }
        public async Task<IEnumerable<Zanr>> GetMovieZanroviDevAsync()
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            String sql = @"select
                           
                           Z.TYPE
                           

                        from public.zanrovi Z 
                        ";

            var result = await conn.QueryAsync<Zanr>(sql);
            return result;
        }

        public async Task<Movie> GetMovieAsync(int id)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            {
                String sql = @"select
                            M.id,
                            M.amount,
                            M.release_date,
                            M.duration,
                            M.title,
                            M.genre,
                            M.director,
                            M.description,
                            M.poster_image,
                            M.price,
                            M.isavailable,
                            M.trailer
                        from public.movie M
                        where M.id=@Id";
                var result = await conn.QueryAsync<Movie>(sql, new { id });
                return result.FirstOrDefault();
            }
        }
        public async Task<Movie> GetMovieForUpdateAsync(int id)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"select 
                                    M.amount,
                            M.release_date,
                            M.duration,
                            M.title,
                            M.genre,
                            M.director,
                            M.description,
                            M.poster_image,
                            M.price,
                            M.id,
                            M.isavailable,
                             M.trailer
                            from public.movie M
                            where M.id=@Id";
                var result = await conn.QueryAsync<Movie>(sql, new { id });
                return result.FirstOrDefault();
            }
        }
        public async Task<Movie> GetMovieDevForUpdateAsync(int id)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"select 
                                    M.amount,
                            M.release_date,
                            M.duration,
                            M.title,
                            Z.type,
                            M.director,
                            M.description,
                            M.poster_image,
                            M.price,
                            M.id,
                            M.isavailable,
                             M.trailer
                            from public.movie M join public.zanrovi Z on M.type=Z.type

                            where M.id=@Id";
                var result = await conn.QueryAsync<Movie>(sql, new { id });
                return result.FirstOrDefault();
            }
        }
        public async Task<int> CreateMovieAsync(CreateMovie newMovie)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"INSERT INTO public.movie(release_date ,duration,title,genre,director,description,poster_image,price,isavailable,trailer)
                        values(@Release_date ,@Duration,@Title,@Genre,@Director,@Description,@Poster_image,@Price,@IsAvailable,@Trailer)
                        returning id";
                return await conn.ExecuteScalarAsync<int>(sql, newMovie);
            }
        }
        public async Task<int> CreateMovieAsync2(CreateMovie newMovie)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                string sql = @"INSERT INTO public.movie(amount, release_date, duration, title, type,director, description, poster_image, price, isavailable, trailer)
                       VALUES (@Amount, @Release_date, @Duration, @Title,@Type, @Director, @Description, @Poster_image, @Price, @IsAvailable, @Trailer)
            
                       RETURNING id";  // Ensure this line returns the new record ID

                var newId = await conn.ExecuteScalarAsync<int>(sql, new
                {
                    Amount = newMovie.Amount,
                    Release_date = newMovie.Release_date,
                    Duration = newMovie.Duration,
                    Title = newMovie.Title,
                    Type=newMovie.Type,
                    Director = newMovie.Director,
                    Description = newMovie.Description,
                    Poster_image = newMovie.Poster_image,  // Pass the byte[] directly
                    Price = newMovie.Price,
                    IsAvailable = newMovie.IsAvailable,
                    Trailer = newMovie.Trailer
                });

                return newId;
            }
        }
        public async Task<long> GetDashInfoAsync()
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                string sql = @"select total_movies();";

                            var result = await conn.ExecuteScalarAsync<long>(sql);

                            return result;
            }
                
        }


        public async Task<int> UpdateMovieAsync(int id, Movie newMovie)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"UPDATE public.movie
                        SET

                            release_date=@Release_date,
                            duration=@Duration,
                            title=@Title,
                            genre=@Genre,
                            director=@Director,
                            description=@Description,
                            poster_image=@Poster_image,
                            price=@Price,
                            isavailable=@IsAvailable,
                            trailer=@Trailer
                           
                        WHERE id=@Id";
             
                return await conn.ExecuteAsync(sql, new { id, newMovie.Release_date,newMovie.Duration,newMovie.Title,newMovie.Genre,newMovie.Director,newMovie.Description,newMovie.Poster_image,newMovie.Price ,newMovie.IsAvailable,newMovie.Trailer});
            }
        }
        public async Task<int> UpdateMovieDevAsync(int id, Movie newMovie)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"UPDATE public.movie
                        SET

                            release_date=@Release_date,
                            duration=@Duration,
                            title=@Title,
                            type=@Type,
                            director=@Director,
                            description=@Description,
                            poster_image=@Poster_image,
                            price=@Price,
                            isavailable=@IsAvailable,
                            trailer=@Trailer
                           
                        WHERE id=@Id";

                return await conn.ExecuteAsync(sql, new { id, newMovie.Release_date, newMovie.Duration, newMovie.Title, newMovie.Type,newMovie.Director, newMovie.Description, newMovie.Poster_image, newMovie.Price, newMovie.IsAvailable, newMovie.Trailer });
            }
        }

        public async Task<int> DeleteMovieAsync(int id)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"DELETE FROM public.movie M
                           WHERE M.id=@Id";
                return await conn.ExecuteAsync(sql, new { id });
            }

        }
        public async Task<IEnumerable<Movie>> SearchMoviesAsync(string searchString)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);

            // Build SQL query with parameters to prevent SQL injection
            string sql = @"SELECT
                        M.amount,
                        M.release_date,
                        M.duration,
                        M.title,
                        M.genre,
                        M.director,
                        M.description,
                        M.poster_image,
                        M.price,
                        M.isavailable,
                         M.trailer
                   FROM public.movie M
                   WHERE M.title ILIKE @SearchString
                      OR M.genre ILIKE @SearchString
                      OR M.director ILIKE @SearchString";

            var parameters = new { SearchString = $"%{searchString}%" };

            var result = await conn.QueryAsync<Movie>(sql, parameters);
            return result;
        }
        public async Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"select
                        M.AMOUNT,
                        M.RELEASE_DATE,
                        M.DURATION,
                        M.TITLE,
                        M.ID,
                        M.GENRE,
                        M.DIRECTOR,
                        M.DESCRIPTION,
                        M.POSTER_IMAGE,
                        M.PRICE,
                        M.ISAVAILABLE,
                        M.TRAILER
                    from public.movie M
                    where M.GENRE = @Genre";
                return await conn.QueryAsync<Movie>(sql, new { Genre = genre });
            }
        }
        public async Task<int?> CreateMovieInCardAsync(CreateMovieInCard newMovieinCard)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String checkSql = @"SELECT id FROM public.buy WHERE id_movie = @Id_movie AND id_user = @Id_user";
                var existingId = await conn.ExecuteScalarAsync<int?>(checkSql, newMovieinCard);
                if (existingId.HasValue)
                {
                    return null; // or return existingId.Value if you want to return the existing record's id
                }


                String sql = @"INSERT INTO public.buy(id_movie,id_user)
                        values(@Id_movie,@Id_user)
                                returning id,id_movie,id_user";
                
                return await conn.ExecuteScalarAsync<int>(sql, newMovieinCard);
            }
        }
       
        public async Task<IEnumerable<Movie>> GetMoviesInCardAsync(int id_user)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            string sql = @"select
                   B.id,
                  B.id_movie,
                  M.amount,
                  M.release_date,
                  M.duration,
                  M.title,
                  M.genre,
                  M.director,
                  M.description,
                  M.poster_image,
                  M.price,
                  M.isavailable,
                   M.trailer
                  from public.buy B
                  join public.user U on U.id=B.id_user
                  join public.movie M on M.id=B.id_movie
                  where B.id_user = @Id_user";
            var result = await conn.QueryAsync<Movie>(sql, new { Id_user = id_user });
            return result;
        }
        public async Task<IEnumerable<Movie>> GetMoviesInProfileAsync(int id_user)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            string sql = @"select
                   B.id,
                  B.id_movie,
                  M.amount,
                  M.release_date,
                  M.duration,
                  M.title,
                  M.genre,
                  M.director,
                  M.description,
                  M.poster_image,
                  M.price,
                  M.isavailable,
                   M.trailer
                  from public.profile B
                  join public.user U on U.id=B.id_user
                  join public.movie M on M.id=B.id_movie
                  where B.id_user = @Id_user";
            var result = await conn.QueryAsync<Movie>(sql, new { Id_user = id_user });
            return result;
        }
        //public async Task<int> DeleteMovieFromCardAsync(int id_movie)
        //{
        //    using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
        //    {
        //        String sql = @"DELETE FROM public.buy B
        //                   WHERE B.id_movie=@Id_movie";
        //        return await conn.ExecuteAsync(sql, new { id_movie });
        //    }

        //}
        public async Task<int> DeleteMovieFromCardAsync(int id)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                string sql = @"DELETE FROM public.buy
               WHERE id=@id;";

                return await conn.ExecuteAsync(sql, new { id });
            }
        }
        public async Task<bool> InsertAndDeleteAllAsync(int id_user)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                await conn.OpenAsync();
                using var transaction = await conn.BeginTransactionAsync();
                try
                {
                    String checkSql = @"SELECT id_movie,id_user,id_show FROM public.buy WHERE id_user = @Id_user";
                    var tickets = await conn.QueryAsync<Buy>(checkSql, new { id_user }, transaction);

                           
                    foreach (var ticket in tickets)
                    {
                       
                            String sql = @"INSERT INTO public.profile(id_movie,id_user,id_show)
                        values(@Id_movie,@Id_user,@Id_show)";
                        await conn.ExecuteAsync(sql, new
                        {
                            id_user = ticket.Id_user,
                            id_movie = ticket.Id_movie,
                            id_show = ticket.Id_show
                        }, transaction);
                    }
                    string deleteSql = @"DELETE FROM public.buy
                        WHERE id_user = @Id_user";
                    await conn.ExecuteAsync(deleteSql, new { id_user }, transaction);
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return false;
                }



            }
        }
        //public async Task<bool> InsertAndDeleteAllAsync(int id_user)
        //{
        //    using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
        //    {

        //        try
        //        {
        //            String checkSql = @"SELECT id_movie,id_user,id_show FROM public.profile WHERE id_user = @Id_user";
        //            var tickets = await conn.QueryAsync<Buy>(checkSql, new { id_user });

        //            foreach (var ticket in tickets)
        //            {
        //                String sql = @"INSERT INTO public.profile(id_movie,id_user,id_show)
        //                values(@Id_movie,@Id_user,@Id_show)
        //                ";
        //                await conn.ExecuteAsync(sql, new
        ////                {
        ////                    id_user = ticket.Id_user,
        ////                    id_movie = ticket.Id_movie,
        ////                    id_show = ticket.Id_show
        ////                }, transaction);

        //            }
        //            string deleteSql = @"DELETE FROM public.buy
        //                WHERE id_user = @Id_user";
        //            await conn.ExecuteAsync(deleteSql, new { id_user }, transaction);
        //            await transaction.CommitAsync();
        //            return true;
        //        }
        //        catch (Exception)
        //        {
        //            await transaction.RollbackAsync();
        //            return false;
        //        }



        //    }
        //}
        //public async Task<int?> CreateMovieInProfileAsync(CreateMovieInProfile newMovieinProfile)
        //{
        //    using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
        //    {
        //        String checkSql = @"SELECT id FROM public.profile WHERE id_movie = @Id_movie AND id_user = @Id_user";
        //        var existingId = await conn.ExecuteScalarAsync<int?>(checkSql, newMovieinProfile);
        //        if (existingId.HasValue)
        //        {
        //            return null; // or return existingId.Value if you want to return the existing record's id
        //        }


        //        String sql = @"INSERT INTO public.profile(id_movie,id_user)
        //                values(@Id_movie,@Id_user)
        //                        returning id,id_movie,id_user";

        //        return await conn.ExecuteScalarAsync<int>(sql, newMovieinProfile);
        //    }
        //}

        public async Task<IEnumerable<Movie>> GetMoviesReportAsync(long? id)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            {
                string sql = @"SELECT
                         M.id,
                        M.amount,
                         M.release_date,
                         M.duration,
                         M.title,
                         M.type,
                         M.director,
                         M.description,
                         M.poster_image,
                         M.price,
                         M.isavailable,
                         M.trailer
                      FROM public.movie M
                      WHERE M.ID = @ID";

                var movies = await conn.QueryAsync<Movie>(sql, new { id });

                // Convert Movie to MovieReports
                var reportData = movies.Select(m => new Movie
                {
                    Id = m.Id,
                    Amount= m.Amount,
                    Release_date = m.Release_date,
                    Duration = m.Duration,
                    Title = m.Title,
                    Type = m.Type,
                    Director = m.Director,
                    Description = m.Description,
                    Poster_image = m.Poster_image,
                    Price = m.Price,
                    IsAvailable = m.IsAvailable,
                    Trailer = m.Trailer
                });

                return reportData;
            }
        }


    }

}
