using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using MoviesAPI.Repositories;
using Newtonsoft.Json;

namespace MoviesAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReviewDevController : ControllerBase
    {
        private readonly ILogger<ReviewDevController> _logger;
        private readonly IReviewRepository _reviewRepository;
        //private readonly IReviewRepository _reviewRepository;

        public ReviewDevController(ILogger<ReviewDevController> logger, IReviewRepository reviewRepository/*,IReviewRepository reviewRepository*/)
        {
            _logger = logger;
            _reviewRepository = reviewRepository;
            //_reviewRepository = reviewRepository;

        }
       

        [HttpGet("ReviewForMovie")]
        public async Task<IActionResult> GetReviewForMovie(int? id)

        {
  
            return Ok(await _reviewRepository.GetReviewForMovieAsync((int)id));
        }
        [HttpGet("ReviewForShow")]
        public async Task<IActionResult> GetReviewForShow(int? id)

        {

            return Ok(await _reviewRepository.GetReviewForShowAsync((int)id));
        }
        [HttpGet("Movies")]
       public async Task<IActionResult> GetReviewMovies()
        {
           var result = await _reviewRepository.GetReviewMovieAsync();

           return Ok(result);
        }
        [HttpGet("Show")]
        public async Task<IActionResult> GetReviewShows()
        {
            var result = await _reviewRepository.GetReviewShowAsync();

            return Ok(result);
        }
        [HttpPost("Movie")]
        public async Task<ActionResult> PostReviewMovie([FromForm] string values)
        {
            if (ModelState.IsValid)
            {
                CreateReview newZad = new CreateReview();
                JsonConvert.PopulateObject(values, newZad);

                var newId = await _reviewRepository.CreateMovieReviewAsync(newZad);

                return Ok(newId);
            }
            else return BadRequest();
        }
        [HttpPost("Show")]
        public async Task<ActionResult> PostReviewShow([FromForm] string values)
        {
            if (ModelState.IsValid)
            {
                CreateReview newZad = new CreateReview();
                JsonConvert.PopulateObject(values, newZad);

                var newId = await _reviewRepository.CreateShowReviewAsync(newZad);

                return Ok(newId);
            }
            else return BadRequest();
        }
        [HttpPut("Movie")]
        public async Task<ActionResult> PutReviewMovie([FromForm] int key, [FromForm] string values)
        {
            if (ModelState.IsValid)
            {
                var existingItem = await _reviewRepository.GetMovieReviewForUpdateAsync(key);

                if (existingItem is null)
                {
                    return NotFound();
                }
                JsonConvert.PopulateObject(values, existingItem);
                var numRecords = await _reviewRepository.UpdateMovieReviewAsync(key, existingItem);

                return Ok(numRecords);
            }
            else return BadRequest();
        }
        [HttpPut("Show")]
        public async Task<ActionResult> PutReviewShow([FromForm] int key, [FromForm] string values)
        {
            if (ModelState.IsValid)
            {
                var existingItem = await _reviewRepository.GetShowReviewForUpdateAsync(key);

                if (existingItem is null)
                {
                    return NotFound();
                }
                JsonConvert.PopulateObject(values, existingItem);
                var numRecords = await _reviewRepository.UpdateShowReviewAsync(key, existingItem);

                return Ok(numRecords);
            }
            else return BadRequest();
        }
        [HttpDelete("Movie")]
        public async Task<ActionResult> DeleteReviewMovie([FromForm] int key)
        {
            var existingItem = await _reviewRepository.GetReviewForMovieAsync(key);

            if (existingItem is null)
            {
                return NotFound();
            }

            try
            {
                var numRecords = await _reviewRepository.DeleteMovieReviewAsync(key);

                return Ok(numRecords);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Show")]
        public async Task<ActionResult> DeleteReviewShow([FromForm] int key)
        {
            var existingItem = await _reviewRepository.GetReviewForShowAsync(key);

            if (existingItem is null)
            {
                return NotFound();
            }

            try
            {
                var numRecords = await _reviewRepository.DeleteShowReviewAsync(key);

                return Ok(numRecords);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
