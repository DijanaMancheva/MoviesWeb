using MoviesAPI.Models;

namespace MoviesAPI.Repositories
{
    public interface ITicketsRepository
    {
        Task<IEnumerable<Tickets>> GetTicketsAsync();
        Task<Tickets> GetTicketsAsync(int id);
        Task<UpdateTickets> GetTicketsForUpdateAsync(int id);
        Task<int> CreateTicketAsync(CreateTicket newZad);
        Task<int> UpdateTicketAsync(int id, UpdateTickets newZad);
        Task<int> DeleteTicketAsync(int id);
        Task UpdateTicketAsync(int id, UpdateTicket updTicket);
    }
}
