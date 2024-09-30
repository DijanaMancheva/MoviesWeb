using MoviesAPI.Models;

namespace MoviesAPI.Repositories
{
    public interface IReviewRepository
    {
        Task<int> CreateMovieReviewAsync(CreateReview newReview);
        Task<int> CreateShowReviewAsync(CreateReview newReview);
        Task<int> DeleteMovieReviewAsync(int id);
        Task<int> DeleteShowReviewAsync(int id);
        Task<UpdateReview> GetMovieReviewForUpdateAsync(int id);
        Task<IEnumerable<Review>> GetReviewForMovieAsync(int id_movie);
        Task<IEnumerable<Review>> GetReviewForMoviesAsync(int id);
        Task<IEnumerable<Review>> GetReviewForShowAsync(int id_show);
        Task<IEnumerable<Review>> GetReviewForShowsAsync(int id);
        Task<IEnumerable<Review>> GetReviewMovieAsync();
        Task<Review> GetReviewMovieAsync(int id);
        Task<IEnumerable<Review>> GetReviewShowAsync();
        Task<Review> GetReviewShowAsync(int id);
        Task<UpdateReview> GetShowReviewForUpdateAsync(int id);
        Task<int> UpdateMovieReviewAsync(int id, UpdateReview newReview);
        Task<int> UpdateShowReviewAsync(int id, UpdateReview newReview);
    }
}