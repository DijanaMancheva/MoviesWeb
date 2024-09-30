using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.PowerBI.Api;
using MoviesWeb.Models;
using Newtonsoft.Json;

namespace MoviesWeb.Controllers
{
    public class MovieController : Controller
    {



        Uri baseAddress = new Uri("https://localhost:44367/api");
        private string data;
        private readonly HttpClient _httpClient;
        private string id;

        public MovieController(IHttpContextAccessor context)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
            id = context.HttpContext.Session.GetString("id");

        }



        [HttpGet]
        public async Task<IActionResult> Search(string searchString)
        {
            List<Movie> movies = new List<Movie>();

            //HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Movie/Search?searchString=" + searchString);
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Movie/Search?searchString=" + searchString);


            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                movies = JsonConvert.DeserializeObject<List<Movie>>(data);
            }

            return View("Index", movies);

        }




        private readonly ILogger<MovieController> _logger;
        //private readonly CxSettings _cxSettings;




        //public IActionResult Index()
        //{
        //    return View();
        //}
        //public async Task<IActionResult> Index()
        //{
        //    List<Movie> movies = new List<Movie>();

        //    HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Movie");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string data = await response.Content.ReadAsStringAsync();
        //        movies = JsonConvert.DeserializeObject<List<Movie>>(data);


        //    }


        //    return View("Index", movies);
        //}
       

        [HttpPost]
        public async Task<IActionResult> AddMovie(Movie model)
        {

            try
            {
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.ImageFile.CopyToAsync(memoryStream);
                        model.Poster_image = memoryStream.ToArray();
                    }
                }

                String data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("IntraApiToken"));
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Movie", content);

                if (response.IsSuccessStatusCode)
                {
                    //TempData["successMessage"] = "Movie Edited.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                //TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();

        }
        [HttpGet]
        public async Task<IActionResult> AddMovie()
        {
            return PartialView("AddMovie");
        }
        [HttpPost]
        public async Task<IActionResult> EditMovie(UpdateMovie model)
        {

            try
            {
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.ImageFile.CopyToAsync(memoryStream);
                        model.Poster_image = memoryStream.ToArray();
                    }
                }

                String data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("IntraApiToken"));
                HttpResponseMessage response = await _httpClient.PutAsync(_httpClient.BaseAddress + "/Movie/" + model.Id, content);
                if (response.IsSuccessStatusCode)
                {
                    //TempData["successMessage"] = "Movie Edited.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                //TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> GetMovieForUpdate(int id)
        {
            Movie movie = new Movie();
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("IntraApiToken"));

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Movie/" + id);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                 movie = JsonConvert.DeserializeObject<Movie>(data);

                return View("EditMovie", movie);
            }
            else
            {
                return View("EditMovie");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMovie(int id)
        {

            try
            {

                HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + "/Movie/Delete?id=" + id);



                if (response.IsSuccessStatusCode)
                {
                    //TempData["successMessage"] = "Movie Edited.";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                // Handle exception (log it, show error message, etc.)
            }
            return RedirectToAction("Index");

        }
        //private bool IsUserAdmin()
        //{
        //    Retrieve the Admin status from session
        //   var isAdmin = HttpContext.Session.GetString("Admin");
        //    return !string.IsNullOrEmpty(isAdmin) && bool.Parse(isAdmin);
        //}
        public async Task<IActionResult> Index(string genre)
        {
            List<Movie> movies = new List<Movie>();

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Movie");

            var currentUser=await GetCurrentUserAsync();
                var isAdmin = currentUser.Admin;
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                movies = JsonConvert.DeserializeObject<List<Movie>>(data);
                //ViewBag.CurrentUser = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
                
            }

            if (!string.IsNullOrEmpty(genre))
            {
                movies = movies.Where(m => m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            ViewData["IsAdmin"] = isAdmin;
            return View("Index", movies);
        }
        private async Task<User> GetCurrentUserAsync()
        {
            User user = new User();
            var id_user = Convert.ToInt32(HttpContext.Session.GetString("id_user"));

            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("IntraApiToken"));

            HttpResponseMessage response =  _httpClient.GetAsync(_httpClient.BaseAddress + "/User/User?id=" + id_user).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<User>(data);
                //ViewBag.CurrentUser = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            }

            return user;

           
        }

        public async Task<IActionResult> GetMoviesInCard()
        {
            List<Movie> movies = new List<Movie>();
            //ViewBag.CurrentUser = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var id_user = Convert.ToInt32(HttpContext.Session.GetString("id_user"));

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Movie/Status?id_user=" + id_user);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                movies = JsonConvert.DeserializeObject<List<Movie>>(data);
            }


            return View("Buy", movies);
        }


        [HttpPost]
        public async Task<IActionResult> Buy(int id)
        {
            try
            {
                var id_user = Convert.ToInt32(HttpContext.Session.GetString("id_user"));

                // Ensure 'data' is a valid JSON string
                string data = JsonConvert.SerializeObject(new { id_movie = id, id_user });

                // Initialize StringContent with the JSON data
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                // Construct the request URI
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Movie/Status?id_movie=" + id + "&id_user=" + id_user, content);

                // Make the POST request with content

                if (response.IsSuccessStatusCode)
                {

                    return RedirectToAction("Buy");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = "The movie is already in your card." });

                    //var errorModel = new ErrorViewModel { ErrorMessage = "The movie is already in your card." };
                    //return View("Index");
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }

            return View("Buy");
        }
        [HttpGet]
        public async Task<IActionResult> Buy()
        {
            List<Movie> movies = new List<Movie>();

            var id_user = Convert.ToInt32(HttpContext.Session.GetString("id_user"));

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Movie/Status?id_user=" + id_user);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                movies = JsonConvert.DeserializeObject<List<Movie>>(data);
                //ViewBag.CurrentUser = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            }
            else
            {
                TempData["errorMessage"] = "Failed to retrive movies";
            }



            return View("Buy", movies);
            //return View("~/Views/Movie/Buy.cshtml");
        }

        [HttpGet, ActionName("DeleteMovieConfirmed")]
        public async Task<IActionResult> DeleteMovieFromCard(int id)
        {

            try
            {

                HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + "/Movie/Status?id=" + id);
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Movie details deleted";
                    //string data = response.Content.ReadAsStringAsync().Result;
                    //buy = JsonConvert.DeserializeObject<Buy>(data);



                    return RedirectToAction("Buy");

                }


            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View("Buy");

        }

        [HttpPost]
        public async Task<IActionResult> ClearUserCard()
        {
            try
            {                             
                var id_user = Convert.ToInt32(HttpContext.Session.GetString("id_user"));
                HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + "/Movie/Clear?id_user=" + id_user);

                    // Call API to clear all movies from the card for the current user

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["successMessage"] = "Payment successful and card cleared!";
                        return RedirectToAction("Index", "Movie"); // Redirect to a relevant page after clearing the card
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to clear the card.";
                    }
                
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return RedirectToAction("Buy");
        }
        public async Task<IActionResult> GetMoviesInProfile()
        {
            List<Movie> movies = new List<Movie>();
            //ViewBag.CurrentUser = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var id_user = Convert.ToInt32(HttpContext.Session.GetString("id_user"));

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Movie/CLear?id_user=" + id_user);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                movies = JsonConvert.DeserializeObject<List<Movie>>(data);
            }


            return View("Profile", movies);
        }

    }


}
    



