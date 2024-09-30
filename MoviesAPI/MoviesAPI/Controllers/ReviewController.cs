using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using MoviesAPI.Repositories;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(ILogger<ReviewController> logger, IReviewRepository reviewRepository)
        {
            _logger = logger;
            _reviewRepository = reviewRepository;
        }

        //// Get all movie reviews
        //[HttpGet("Movies")]
        //public async Task<IActionResult> GetReviewMovies()
        //{
        //    var result = await _reviewRepository.GetReviewMovieAsync();
        //    return Ok(result);
        //}

        //// Get all show reviews
        //[HttpGet("Shows")]
        //public async Task<IActionResult> GetReviewShows()
        //{
        //    var result = await _reviewRepository.GetReviewShowAsync();
        //    return Ok(result);
        //}

        // Get reviews for a specific movie by ID
        [HttpGet("Movies")]
        public async Task<ActionResult> GetReviewForMovies(int id_movie)
        {
            var filteredReviews = await _reviewRepository.GetReviewForMovieAsync(id_movie);
            if (filteredReviews == null)
            {
                return NotFound("No reviews found for this movie.");
            }
            return Ok(filteredReviews);
        }
        [HttpGet("MovieReview")]
        public async Task<ActionResult> GetReviewForMovie(int id)
        {
            var filteredReviews = await _reviewRepository.GetMovieReviewForUpdateAsync(id);
            if (filteredReviews == null)
            {
                return NotFound("No reviews found for this movie.");
            }
            return Ok(filteredReviews);
        }
        [HttpGet("ShowReview")]
        public async Task<ActionResult> GetReviewForShow(int id)
        {
            var filteredReviews = await _reviewRepository.GetShowReviewForUpdateAsync(id);
            if (filteredReviews == null)
            {
                return NotFound("No reviews found for this movie.");
            }
            return Ok(filteredReviews);
        }

        // Get reviews for a specific show by ID
        [HttpGet("Shows")]
        public async Task<ActionResult> GetReviewForShows(int id_show)
        {
            var filteredReviews = await _reviewRepository.GetReviewForShowAsync(id_show);
            if (filteredReviews == null)
            {
                return NotFound("No reviews found for this show.");
            }
            return Ok(filteredReviews);
        }

        // Create a new movie review
        [HttpPost("movies")]
        public async Task<ActionResult> PostReviewMovie(CreateReview newReview)
        {
            if (ModelState.IsValid)
            {
                var newId = await _reviewRepository.CreateMovieReviewAsync(newReview);
                return Ok(newId);
            }
            return BadRequest();
        }

        // Create a new show review
        [HttpPost("shows")]
        public async Task<ActionResult> PostReviewShow(CreateReview newReview)
        {
            if (ModelState.IsValid)
            {
                var newId = await _reviewRepository.CreateShowReviewAsync(newReview);
                return Ok(newId);
            }
            return BadRequest();
        }

        // Update a movie review
        [HttpPut("movies")]
        public async Task<ActionResult> PutReviewMovie(int id, UpdateReview updReview)
        {
            if (ModelState.IsValid)
            {
                var existingItem = await _reviewRepository.GetMovieReviewForUpdateAsync(id);
                if (existingItem == null)
                {
                    return NotFound();
                }

                var numRecords = await _reviewRepository.UpdateMovieReviewAsync(id, updReview);
                return Ok(numRecords);
            }
            return BadRequest();
        }

        // Update a show review
        [HttpPut("shows")]
        public async Task<ActionResult> PutReviewShow(int id, UpdateReview updReview)
        {
            if (ModelState.IsValid)
            {
                var existingItem = await _reviewRepository.GetShowReviewForUpdateAsync(id);
                if (existingItem == null)
                {
                    return NotFound();
                }

                var numRecords = await _reviewRepository.UpdateShowReviewAsync(id, updReview);
                return Ok(numRecords);
            }
            return BadRequest();
        }

        // Delete a movie review
        [HttpDelete("movies/{id}")]
        public async Task<ActionResult> DeleteReviewMovie(int id)
        {
            var existingItem = await _reviewRepository.GetReviewMovieAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            try
            {
                var numRecords = await _reviewRepository.DeleteMovieReviewAsync(id);
                return Ok(numRecords);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Delete a show review
        [HttpDelete("shows/{id}")]
        public async Task<ActionResult> DeleteReviewShow(int id)
        {
            var existingItem = await _reviewRepository.GetReviewShowAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            try
            {
                var numRecords = await _reviewRepository.DeleteShowReviewAsync(id);
                return Ok(numRecords);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
