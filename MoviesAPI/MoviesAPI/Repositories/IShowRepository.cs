using MoviesAPI.Models;

namespace MoviesAPI.Repositories
{
    public interface IShowRepository
    {
        Task<int> CreateShowAsync(CreateShow newShow);
        Task<int> CreateShowAsync2(CreateShow newShow);
        Task<int?> CreateShowInCardAsync(CreateShowInCard newShowinCard);
        Task<int> DeleteShowAsync(int id);
        Task<int> DeleteShowFromCardAsync(int id);
        Task<long> GetDashInfoAsync();
        Task<IEnumerable<Show>> GetShowAsync();
        Task<Show> GetShowAsync(int id);
        Task<Show> GetShowForUpdateAsync(int id);
        Task<IEnumerable<Show>> GetShowsByGenreAsync(string genre);
        Task<IEnumerable<Show>> GetShowsInCardAsync(int id_user);
        Task<IEnumerable<Show>> GetShowsInProfileAsync(int id_user);
        Task<IEnumerable<Show>> GetShowsReportAsync(long? id);
        Task<bool> InsertAndDeleteAllAsync(int id_user);
        Task<IEnumerable<Show>> SearchShowsAsync(string searchString);
        Task<int> UpdateShowAsync(int id, Show newShow);
    }
}