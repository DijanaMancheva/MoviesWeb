using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using MoviesAPI.Repositories;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowController : Controller
    {
        private readonly ILogger<ShowController> _logger;
        private readonly IShowRepository _showRepository;
        
        public ShowController(ILogger<ShowController> logger, IShowRepository showRepository)
        {
            _logger = logger;
            _showRepository = showRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _showRepository.GetShowAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetShow(int id)
        {
            var item = await _showRepository.GetShowAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            else return Ok(item);
        }
        [HttpPost]
        public async Task<ActionResult> PostSHow(CreateShow newShow)
        {
            if (ModelState.IsValid)
            {

                var newId = await _showRepository.CreateShowAsync(newShow);
                return Ok(newId);
            }
            else return BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutShow(int id, Show updShow)
        {
            if (ModelState.IsValid)
            {
                var existingItem = await _showRepository.GetShowForUpdateAsync(id);
                if (existingItem == null)
                {
                    return NotFound();
                }

                var numRecords = await _showRepository.UpdateShowAsync(id, updShow);

                return Ok(numRecords);
            }
            else return BadRequest();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var existingItem = await _showRepository.GetShowAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            try
            {
                var numRecords = await _showRepository.DeleteShowAsync(id);
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
            var filteredShows = await _showRepository.SearchShowsAsync(searchString);
            if (filteredShows == null || !filteredShows.Any())
            {
                return NotFound("No movies found matching the query.");
            }
            return Ok(filteredShows);
        }
        [HttpPost("Status")]
        public async Task<ActionResult> PostShowInCard(CreateShowInCard newShowinCard)
        {
            if (ModelState.IsValid)
            {

                int? newId = await _showRepository.CreateShowInCardAsync(newShowinCard);
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
        public async Task<ActionResult> GetShowsInCard(int id_user)
        {
            var result = await _showRepository.GetShowsInCardAsync(id_user);
            return Ok(result);
        }
        [HttpDelete("Status")]
        public async Task<IActionResult> DeleteShowFromCart(int id)
        {


            try
            {
                // Attempt to remove the movie with the specified ID from the user's cart
                long numRecordsAffected = await _showRepository.DeleteShowFromCardAsync(id);

                if (numRecordsAffected > 0)
                {
                    return Ok(new { success = true, message = "Show removed from the user's cart successfully." });
                }

                return NotFound(new { success = false, message = "Show not found in the user's cart." });
            }
            catch (Exception)
            {
                // Handle any exceptions and return an appropriate response
                return StatusCode(500, "An error occurred while removing the show from the user's cart.");
            }
        }
        [HttpGet("Clear")]
        public async Task<ActionResult> GetShowsInProfile(int id_user)
        {
            var result = await _showRepository.GetShowsInProfileAsync(id_user);
            return Ok(result);
        }
        [HttpDelete("Clear")]
        public async Task<ActionResult> ClearUserCart(int id_user)
        {

            // Call the repository method to clear all movies from the user's cart
            var success = await _showRepository.InsertAndDeleteAllAsync(id_user);

            if (success)
            {
                return Ok(new { success = true, message = "All movies cleared from the user's cart." });
            }
            else
            {
                return NotFound(new { success = false, message = "No movies found in the user's cart." });

            }


        }
    }
}
