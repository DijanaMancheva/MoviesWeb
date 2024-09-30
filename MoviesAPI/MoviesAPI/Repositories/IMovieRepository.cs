using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;

namespace MoviesAPI.Repositories
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetMovieAsync();
        Task<Movie> GetMovieAsync(int id);
        //Task<UpdateMovie> GetMovieForUpdateAsync(int id);
        Task<int> CreateMovieAsync(CreateMovie newMovie);
        Task<int> DeleteMovieAsync(int id);
        Task<IEnumerable<Movie>> SearchMoviesAsync(string searchString);
        Task<int?> CreateMovieInCardAsync(CreateMovieInCard newMovieinCard);
        Task<IEnumerable<Movie>> GetMoviesInCardAsync(int id_user);
        Task<int> DeleteMovieFromCardAsync(int id);
        //Task<IEnumerable<Buy>> ClearUserCartAsync(int id_user);
        //Task<int?> CreateMovieInProfileAsync(CreateMovieInProfile newMovieinProfile);

        Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre);
        Task<bool> InsertAndDeleteAllAsync(int id_user);
        Task<IEnumerable<Movie>> GetMoviesInProfileAsync(int id_user);
        Task<IEnumerable<Movie>> GetMoviesReportAsync(long? id);
        Task<Movie> GetMovieDevAsync(int id);
        Task<long> GetDashInfoAsync();
        Task<int> CreateMovieAsync2(CreateMovie newMovie);
        Task<int> UpdateMovieAsync(int id, Movie newMovie);
        Task<Movie> GetMovieForUpdateAsync(int id);
        Task<IEnumerable<Movie>> GetMovieDevAsync();
        Task<Movie> GetMovieDevForUpdateAsync(int id);
        Task<int> UpdateMovieDevAsync(int id, Movie newMovie);
        Task<IEnumerable<Zanr>> GetMovieZanroviDevAsync();
    }
}
