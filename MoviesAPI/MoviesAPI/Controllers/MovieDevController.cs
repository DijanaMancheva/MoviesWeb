using FastReport.Export.PdfSimple;
using FastReport;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using MoviesAPI.Repositories;
using Newtonsoft.Json;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieDevController : ControllerBase
    {
        private readonly ILogger<MovieDevController> _logger;
        private readonly IMovieRepository _movieRepository;
        //private readonly IReviewRepository _reviewRepository;
        private readonly IWebHostEnvironment _env;

        public MovieDevController(ILogger<MovieDevController> logger, IMovieRepository movieRepository/*,IReviewRepository reviewRepository*/, IWebHostEnvironment env)
        {
            _logger = logger;
            _movieRepository = movieRepository;
            _env = env;
            //_reviewRepository = reviewRepository;

        }
        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var result = await _movieRepository.GetMovieDevAsync();
            return Ok(result);
        }
        [HttpGet("zanrovi")]
        public async Task<IActionResult> GetMoviesZanrovi()
        {
            var result = await _movieRepository.GetMovieZanroviDevAsync();
            return Ok(result);
        }

        [HttpGet("Filter")]
        public async Task<IActionResult> GetMovies(DateTime datumOd, DateTime datumDo)
        {
            // Await the result of the asynchronous operation
            var movies = await _movieRepository.GetMovieDevAsync(); // Make sure this method returns Task<IEnumerable<Movie>>

            // Now apply the LINQ filter on the resulting IEnumerable<Movie>
            var filteredMovies = movies
                .Where(m => m.Release_date >= datumOd && m.Release_date <= datumDo)
                .ToList();

            return Ok(filteredMovies);
        }
        [HttpGet("GetMovies")]
        public async Task<ActionResult> GetMovies(int id)
        {
            var item = await _movieRepository.GetMovieDevAsync(id);
            if (item is null)
                return NotFound();
            else
                return Ok(item);
        }
        //[HttpPost]

        //public async Task<ActionResult> Post([FromForm] CreateMovie proekt)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //CreatePraksaUsers newUser = new CreatePraksaUsers();


        //        var newId = await _movieRepository.CreateMovieAsync(proekt);

        //        return Ok(newId);
        //    }
        //    else return BadRequest();
        //}
        [HttpPost]
        public async Task<ActionResult> PostMovie([FromForm] CreateMovie newMovie, [FromForm] IFormFile posterFile)
        {
            if (ModelState.IsValid)
            {
                if (posterFile != null && posterFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await posterFile.CopyToAsync(memoryStream);
                        newMovie.Poster_image = memoryStream.ToArray();
                    }
                }

                var newId = await _movieRepository.CreateMovieAsync2(newMovie);
                return Ok(newId);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet("procedura")]
        public async Task<ActionResult> Get()
        {
           

            return Ok(await _movieRepository.GetDashInfoAsync());
        }
        [HttpPut]
        public async Task<ActionResult> PutMovie(int id,[FromForm] Movie values,[FromForm] IFormFile? posterFile)
        {
            if (ModelState.IsValid)
            {
                var existingItem = await _movieRepository.GetMovieDevForUpdateAsync(id);

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
                var numRecords = await _movieRepository.UpdateMovieDevAsync(id, values);


                return Ok(numRecords);
            }
            else return BadRequest();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteMovie([FromForm] int key)
        {
            var existingItem = await _movieRepository.GetMovieAsync(key);

            if (existingItem is null)
            {
                return NotFound();
            }

            try
            {
                var numRecords = await _movieRepository.DeleteMovieAsync(key);

                return Ok(numRecords);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("Report")]
        public async Task<IActionResult> GetMoviesReport(long? id)
        {
            var results = await _movieRepository.GetMoviesReportAsync(id)

        ;
            return Ok(results);
        }
        //[HttpGet("Report")]
        //public async Task<ActionResult> GetMoviesReport(int id)
        //{
        //    //string thisFolder = Config.ApplicationFolder;

        //    //string reportsFolder = FindReportsFolder();

        //    Report report = new Report();

        //    Movie movie = await _movieRepository.GetMovieAsync(id);
        //    List<Movie> data = new List<Movie> { movie };

           

        //    //report.Load(Path.Combine(_env.WebRootPath, "reports", "raboten_nalog.frx"));

        //    //JsonDataSourceConnection jsonDataSourceConnection = new JsonDataSourceConnection();
        //    //jsonDataSourceConnection. = ToJson(data);
        //    //report.Dictionary.Connections.Add(jsonDataSourceConnection);

        //    report.RegisterData(data, "qry");

        //    //report.GetDataSource("Connection").Enabled = true;

        //    if (report.Prepare())
        //    {
        //        PDFSimpleExport pdfExport = new PDFSimpleExport();

        //        MemoryStream strm = new MemoryStream();
        //        report.Export(pdfExport, strm);
        //        report.Dispose();

        //        strm.Position = 0;

        //        // return stream in browser
        //        return File(strm, "application/pdf", "movies.pdf");
        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error preparing report.");
        //    }
        //}
    }
}
