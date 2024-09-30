using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using MoviesAPI.Repositories;
using Newtonsoft.Json;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowDevController : ControllerBase
    {
        private readonly ILogger<ShowDevController> _logger;
        private readonly IShowRepository _showRepository;
        //private readonly IReviewRepository _reviewRepository;

        public ShowDevController(ILogger<ShowDevController> logger, IShowRepository showRepository/*,IReviewRepository reviewRepository*/)
        {
            _logger = logger;
            _showRepository = showRepository;
            //_reviewRepository = reviewRepository;

        }
        [HttpGet]
        public async Task<IActionResult> GetShows()
        {
            var result = await _showRepository.GetShowAsync();
            return Ok(result);
        }
        [HttpGet("procedura")]
        public async Task<ActionResult> Get()
        {


            return Ok(await _showRepository.GetDashInfoAsync());
        }
        [HttpGet("Filter")]
        public async Task<IActionResult> GetShows(DateTime datumOd, DateTime datumDo)
        {
            // Await the result of the asynchronous operation
            var movies = await _showRepository.GetShowAsync(); // Make sure this method returns Task<IEnumerable<Movie>>

            // Now apply the LINQ filter on the resulting IEnumerable<Movie>
            var filteredMovies = movies
                .Where(m => m.Release_date >= datumOd && m.Release_date <= datumDo)
                .ToList();

            return Ok(filteredMovies);
        }
       
        [HttpGet("GetShows")]
        public async Task<ActionResult> GetShows(int id)
        {
            var item = await _showRepository.GetShowAsync(id);
            if (item is null)
                return NotFound();
            else
                return Ok(item);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetShows2(int id)
        {
            var item = await _showRepository.GetShowAsync(id);
            if (item is null)
                return NotFound();
            else
                return Ok(item);
        }
        //[HttpPost]
        //public async Task<ActionResult> PostShow([FromForm] string values)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        CreateShow newZad = new CreateShow();
        //        JsonConvert.PopulateObject(values, newZad);

        //        var newId = await _showRepository.CreateShowAsync(newZad);

        //        return Ok(newId);
        //    }
        //    else return BadRequest();
        //}
        [HttpPut]
        public async Task<ActionResult> PutShow(int id, [FromForm] Show values, [FromForm] IFormFile posterFile)
        {
            if (ModelState.IsValid)
            {
                var existingItem = await _showRepository.GetShowForUpdateAsync(id);

                if (existingItem is null)
                {
                    return NotFound();
                }
                if (posterFile != null && posterFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await posterFile.CopyToAsync(memoryStream);
                        values.Poster_image = memoryStream.ToArray();
                    }
                }
                var numRecords = await _showRepository.UpdateShowAsync(id, values);

                return Ok(numRecords);
            }
            else return BadRequest();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteShow([FromForm] int key)
        {
            var existingItem = await _showRepository.GetShowAsync(key);

            if (existingItem is null)
            {
                return NotFound();
            }

            try
            {
                var numRecords = await _showRepository.DeleteShowAsync(key);

                return Ok(numRecords);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("Report")]
        public async Task<IActionResult> GetShowsReport(long? id)
        {
            var results = await _showRepository.GetShowsReportAsync(id)

        ;
            return Ok(results);
        }
        [HttpPost]
        public async Task<ActionResult> PostShow([FromForm] CreateShow newShow, [FromForm] IFormFile posterFile)
        {
            if (ModelState.IsValid)
            {
                if (posterFile != null && posterFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await posterFile.CopyToAsync(memoryStream);
                        newShow.Poster_image = memoryStream.ToArray();
                    }
                }

                var newId = await _showRepository.CreateShowAsync2(newShow);
                return Ok(newId);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
