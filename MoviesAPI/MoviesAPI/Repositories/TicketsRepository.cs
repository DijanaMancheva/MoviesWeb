using Microsoft.Extensions.Options;
using System.Runtime;
using MoviesAPI.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace MoviesAPI.Repositories
{
    public class TicketsRepository : ITicketsRepository
    {
        private readonly MoviesDB _dbSettings;
        public TicketsRepository(IOptions<MoviesDB> dbSettings)
        {
            _dbSettings = dbSettings.Value;
        }
        public async Task<IEnumerable<Tickets>> GetTicketsAsync()
        {
            using NpgsqlConnection conn = new(_dbSettings.DefaultConnection) ;
            String sql = @"select 
                               T.amount,
                               T.watch_movie,
                               T.user_id,
                               T.movie_id,
                                T.id
                    from public.tickets T";
            var result = await conn.QueryAsync<Tickets>(sql);
            return result;
        }
        public async Task<Tickets> GetTicketsAsync(int id)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"select 
                                 T.amount,
                               T.watch_movie,
                               T.user_id,
                               T.movie_id,
                                T.id
                    from public.tickets T
                    where T.id=@Id";
                var result = await conn.QueryAsync<Tickets>(sql, new { id });
                return result.FirstOrDefault();
            }
            }
        public async Task<UpdateTickets> GetTicketsForUpdateAsync(int id)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection)) 
            {
                String sql = @"select 
                                T.amount,
                               T.watch_movie,
                               T.user_id,
                               T.movie_id
                    from public.tickets T
                    where T.id=@Id";
                var result=await conn.QueryAsync<UpdateTickets>(sql, new { id });
                return result.FirstOrDefault();
            }
        }
        public async Task<int> CreateTicketAsync(CreateTicket newZad)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql= @"INSERT INTO public.tickets(amount,watch_movie,user_id,movie_id,id)
                    values(@Amount,@Watch_movie,@User_id,@Movie_id,@Id)
                    returning id";
                return await conn.ExecuteScalarAsync<int>(sql, newZad);
            }

        }
        public async Task<int> UpdateTicketAsync(int id,UpdateTickets newZad)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"UPDATE public.tickets
                            SET amount=@Amount,
                                watch_movie=@Watch_movie,
                                user_id=@User_id,
                                movie_id=@Movie_id
                           WHERE id=@Id";
                return await conn.ExecuteAsync(sql,new { id, newZad.Watch_movie, newZad.User_id, newZad.Movie_id, newZad.Amount });

            }
        }
        public async Task<int> DeleteTicketAsync(int id)
        {
            using (NpgsqlConnection conn = new(_dbSettings.DefaultConnection))
            {
                String sql = @"DELETE FROM public.tickets T
                                WHERE T.id=@Id";
                return await conn.ExecuteAsync(sql, new { id });
            }

        }

        public Task UpdateTicketAsync(int id, UpdateTicket updTicket)
        {
            throw new NotImplementedException();
        }

        //public Task<int> UpdateTicketAsync(int id, UpdateTicket newZad)
        //{
        //    throw new NotImplementedException();
        //}

        //Task ITicketsRepository.UpdateTicketAsync(int key, UpdateTickets existingItem)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
