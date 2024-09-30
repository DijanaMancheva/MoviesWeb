using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MoviesAPI.Models;
using Npgsql;

namespace MoviesAPI.Repositories
{
    public class ShowRepository : IShowRepository
    {

        private readonly MoviesDB _dbSettings;
        private readonly IShowRepository _showRepository;
        public ShowRepository(IOptions<MoviesDB> dbSettings)
        {
            _dbSettings = dbSettings.Value;

        }
      public async Task<IEnumerable<Show>> GetShowAsync()
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            String sql = @"select 
                            S.ID,
                            S.RELEASE_DATE,
                            S.SEASONS,
                            S.TITLE,
                            S.GENRE,
                            S.DIRECTOR,
                            S.DESCRIPTION,
                            S.POSTER_IMAGE,
                            S.PRICE,
                            S.ISAVAILABLE
                            from public.show S";
            var result = await conn.QueryAsync<Show>(sql);
            return result;
                
                
        }
        public async Task<Show> GetShowAsync(int id)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            {
                String sql = @"select
                            S.id,
                            S.release_date,
                            S.seasons,
                            S.title,
                            S.genre,
                            S.director,
                            S.description,
                            S.poster_image,
                            S.price,
                            S.isavailable
                        from public.show S
                        where S.id=@Id";
                var result = await conn.QueryAsync<Show>(sql, new {id});
                return result.FirstOrDefault();
            }

        }
        public async Task<long> GetDashInfoAsync()
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                string sql = @"select total_shows();";

                var result = await conn.ExecuteScalarAsync<long>(sql);

                return result;
            }

        }
        public async Task<Show> GetShowForUpdateAsync(int id)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"select 
                                    
                            S.release_date,
                            S.seasons,
                            S.title,
                            S.genre,
                            S.director,
                            S.description,
                            S.poster_image,
                            S.price,
                            S.id,
                            S.isavailable
                            from public.show S
                            where S.id=@Id";
                var result = await conn.QueryAsync<Show>(sql, new { id });
                return result.FirstOrDefault();
            }
        }
        public async Task<int> CreateShowAsync(CreateShow newShow)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"INSERT INTO public.show(release_date ,seasons,title,genre,director,description,poster_image,price,isavailable)
                        values(@Release_date ,@Seasons,@Title,@Genre,@Director,@Description,@Poster_image,@Price,@IsAvailable)
                        returning id";
                return await conn.ExecuteScalarAsync<int>(sql, newShow);
            }
        }
        public async Task<int> UpdateShowAsync(int id, Show newShow)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"UPDATE public.show
                        SET
                            release_date=@Release_date,
                            seasons=@Seasons,
                            title=@Title,
                            genre=@Genre,
                            director=@Director,
                            description=@Description,
                            poster_image=@Poster_image,
                            price=@Price,
                            isavailable=@IsAvailable,
                            trailer=@Trailer
                           
                        WHERE id=@Id";
                return await conn.ExecuteAsync(sql, new { id, newShow.Release_date, newShow.Seasons, newShow.Title, newShow.Genre, newShow.Director, newShow.Description,newShow.Poster_image, newShow.Price, newShow.IsAvailable ,newShow.Trailer});
            }
        }
        public async Task<int> DeleteShowAsync(int id)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"DELETE FROM public.show S
                           WHERE S.id=@Id";
                return await conn.ExecuteAsync(sql, new { id });
            }

        }
        public async Task<IEnumerable<Show>> SearchShowsAsync(string searchString)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);

            // Build SQL query with parameters to prevent SQL injection
            string sql = @"SELECT
                       
                        S.release_date,
                        S.seasons,
                        S.title,
                        S.genre,
                        S.director,
                        S.description,
                        S.poster_image,
                        S.price,
                        S.isavailable
                   FROM public.show S
                   WHERE S.title ILIKE @SearchString
                      OR S.genre ILIKE @SearchString
                      OR S.director ILIKE @SearchString";

            var parameters = new { SearchString = $"%{searchString}%" };

            var result = await conn.QueryAsync<Show>(sql, parameters);
            return result;
        }
        public async Task<IEnumerable<Show>> GetShowsByGenreAsync(string genre)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"select
                       
                         S.release_date,
                        S.seasons,
                        S.title,
                        S.genre,
                        S.director,
                        S.description,
                        S.poster_image,
                        S.price,
                        S.isavailable
                   FROM public.show S
                    where S.genre = @Genre";
                return await conn.QueryAsync<Show>(sql, new { Genre = genre });
            }
        }
        public async Task<int?> CreateShowInCardAsync(CreateShowInCard newShowinCard)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String checkSql = @"SELECT id FROM public.buy WHERE id_show = @Id_show AND id_user = @Id_user";
                var existingId = await conn.ExecuteScalarAsync<int?>(checkSql, newShowinCard);
                if (existingId.HasValue)
                {
                    return null; // or return existingId.Value if you want to return the existing record's id
                }


                String sql = @"INSERT INTO public.buy(id_show,id_user)
                        values(@Id_show,@Id_user)
                                returning id,id_show,id_user";

                return await conn.ExecuteScalarAsync<int>(sql, newShowinCard);
            }
        }
        public async Task<IEnumerable<Show>> GetShowsInCardAsync(int id_user)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            string sql = @"select
                   B.id,
                  B.id_show,
                  S.release_date,
                  S.seasons,
                  S.title,
                  S.genre,
                  S.director,
                  S.description,
                  S.poster_image,
                  S.price,
                  S.isavailable
                  from public.buy B
                  join public.user U on U.id=B.id_user
                  join public.show S on S.id=B.id_show
                  where B.id_user = @Id_user";
            var result = await conn.QueryAsync<Show>(sql, new { Id_user = id_user });
            return result;
        }
        public async Task<int> DeleteShowFromCardAsync(int id)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                string sql = @"DELETE FROM public.buy
               WHERE id=@id;";

                return await conn.ExecuteAsync(sql, new { id });
            }
        }
        public async Task<IEnumerable<Show>> GetShowsInProfileAsync(int id_user)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            string sql = @"select
                   B.id,
                  B.id_show,
                  M.release_date,
                  M.seasons,
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
                  join public.show M on M.id=B.id_show
                  where B.id_user = @Id_user";
            var result = await conn.QueryAsync<Show>(sql, new { Id_user = id_user });
            return result;
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
        public async Task<IEnumerable<Show>> GetShowsReportAsync(long? id)
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection);
            {
                string sql = @"SELECT
                         M.id,
                         M.release_date,
                         M.seasons,
                         M.title,
                         M.genre,
                         M.director,
                         M.description,
                         M.poster_image,
                         M.price,
                         M.isavailable,
                         M.trailer
                      FROM public.show M
                      WHERE M.ID = @ID";

                var shows = await conn.QueryAsync<Show>(sql, new { id });

                // Convert Movie to MovieReports
                var reportData = shows.Select(m => new Show
                {
                    Id = m.Id,
                    Release_date = m.Release_date,
                    Seasons = m.Seasons,
                    Title = m.Title,
                    Genre = m.Genre,
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
        public async Task<int> CreateShowAsync2(CreateShow newShow)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                string sql = @"INSERT INTO public.show( release_date, seasons, title, genre, director, description, poster_image, price, isavailable, trailer)
                       VALUES (@Release_date, @Seasons, @Title, @Genre, @Director, @Description, @Poster_image, @Price, @IsAvailable, @Trailer)
                       RETURNING id";  // Ensure this line returns the new record ID

                var newId = await conn.ExecuteScalarAsync<int>(sql, new
                {
                    Release_date = newShow.Release_date,
                    Seasons = newShow.Seasons,
                    Title = newShow.Title,
                    Genre = newShow.Genre,
                    Director = newShow.Director,
                    Description = newShow.Description,
                    Poster_image = newShow.Poster_image,  // Pass the byte[] directly
                    Price = newShow.Price,
                    IsAvailable = newShow.IsAvailable,
                    Trailer = newShow.Trailer
                });

                return newId;
            }
        }

    }

}
