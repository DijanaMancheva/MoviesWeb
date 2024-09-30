using FastReport.Export.PdfSimple;
using FastReport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using MoviesAPI.Repositories;
using Newtonsoft.Json;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IMovieRepository _movieRepository;
        //private readonly IReviewRepository _reviewRepository;

        public MovieController(ILogger<MovieController> logger, IMovieRepository movieRepository/*,IReviewRepository reviewRepository*/)
        {
            _logger = logger;
            _movieRepository = movieRepository;
            //_reviewRepository = reviewRepository;

        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _movieRepository.GetMovieAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetMovie(int id)
        {
            var item = await _movieRepository.GetMovieAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            else return Ok(item);
        }
        [HttpPost]
        public async Task<ActionResult> PostMovie(CreateMovie newMovie)
        {
            if (ModelState.IsValid)
            {

                var newId = await _movieRepository.CreateMovieAsync(newMovie);
                return Ok(newId);
            }
            else return BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutMovie(int id, Movie updMovie)
        {
            if (ModelState.IsValid)
            {
                var existingItem = await _movieRepository.GetMovieForUpdateAsync(id);
                if (existingItem == null)
                {
                    return NotFound();
                }
                var numRecords = await _movieRepository.UpdateMovieAsync(id, updMovie);

                return Ok(numRecords);
            }
            else return BadRequest();
        }
    
        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteMovie( int id)
        {
            var existingItem = await _movieRepository.GetMovieAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            try
            {
                var numRecords = await _movieRepository.DeleteMovieAsync(id);
                return Ok(numRecords);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string searchString)
        {
            var filteredMovies = await _movieRepository.SearchMoviesAsync(searchString);
            if (filteredMovies == null || !filteredMovies.Any())
            {
                return NotFound("No movies found matching the query.");
            }
            return Ok(filteredMovies);
        }
        [HttpPost("Status")]
        public async Task<ActionResult> PostMovieInCard(CreateMovieInCard newMovieinCard)
        {
            if (ModelState.IsValid)
            {

                int? newId = await _movieRepository.CreateMovieInCardAsync(newMovieinCard);
                if (newId == null)
                {
                    // Handle the case where the movie is already in the card
                    return BadRequest("The movie is already in your card.");
                }
                return Ok(newId);
            }
            else return BadRequest();
        }
        [HttpGet("Status")]
        public async Task<ActionResult> GetMoviesInCard(int id_user)
        {
            var result = await _movieRepository.GetMoviesInCardAsync(id_user);
            return Ok(result);
        }
        [HttpGet("Clear")]
        public async Task<ActionResult> GetMoviesInProfile(int id_user)
        {
            var result = await _movieRepository.GetMoviesInProfileAsync(id_user);
            return Ok(result);
        }
        //[HttpGet("GetMoviesInCard")]
        //public async Task<ActionResult> GetMoviesInCardProfile(int id_user)
        //{
        //    try
        //    {
        //        var movieIds = await _movieRepository.GetMoviesInCardAsync(id_user);
        //        if (movieIds != null)
        //        {
        //            return Ok(movieIds);
        //        }
        //        return NotFound("No movies in the user's card");
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "An error occurred while fetching movies from the card.");
        //    }

        //}

        //[HttpDelete("Status")]
        //public async Task<ActionResult> DeleteMovieFromCard(int id_movie)
        //{
        //    var existingItem = await _movieRepository.GetMoviesInCardAsync(id);
        //    if (existingItem == null)
        //    {
        //        return NotFound();
        //    }
        //    try
        //    {
        //        var numRecords = await _movieRepository.DeleteMovieFromCardAsync(id_movie);
        //        return Ok(numRecords);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}
        [HttpDelete("Status")]
        public async Task<IActionResult> DeleteMovieFromCart(int id)
        {
            

            try
            {
                // Attempt to remove the movie with the specified ID from the user's cart
                long numRecordsAffected = await _movieRepository.DeleteMovieFromCardAsync(id);

                if (numRecordsAffected > 0)
                {
                    return Ok(new { success = true, message = "Movie removed from the user's cart successfully." });
                }

                return NotFound(new { success = false, message = "Movie not found in the user's cart." });
            }
            catch (Exception)
            {
                // Handle any exceptions and return an appropriate response
                return StatusCode(500, "An error occurred while removing the movie from the user's cart.");
            }
        }
        [HttpDelete("Clear")]
        public async Task<ActionResult> ClearUserCart(int id_user)
        {
           
                // Call the repository method to clear all movies from the user's cart
                var success = await _movieRepository.InsertAndDeleteAllAsync(id_user);

                if (success)
                {
                    return Ok(new { success = true, message = "All movies cleared from the user's cart." });
                }
            else
            {
                return NotFound(new { success = false, message = "No movies found in the user's cart." });

            }
            
           
        }

        //[HttpPost("Profile")]
        //public async Task<ActionResult> PostMovieInProfile(CreateMovieInProfile newMovieinProfile)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        int? newId = await _movieRepository.CreateMovieInProfileAsync(newMovieinProfile);
        //        if (newId == null)
        //        {
        //            // Handle the case where the movie is already in the card
        //            return BadRequest("The movie is already in your card.");
        //        }
        //        return Ok(newId);
        //    }
        //    else return BadRequest();
        //}

        

    }
}