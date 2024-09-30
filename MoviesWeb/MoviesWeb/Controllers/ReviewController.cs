using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MoviesWeb.Models;
using Newtonsoft.Json;

namespace MoviesWeb.Controllers
{
    public class ReviewController : Controller
    {

        Uri baseAddress = new Uri("https://localhost:44367/api");
        private string data;
        private readonly HttpClient _httpClient;
        
        public ReviewController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        public async Task<IActionResult> Index(int id)
        {
            List<Review> reviews = new List<Review>();
            string data = JsonConvert.SerializeObject(new { id_movie = id});

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Review/Movies?id_movie="+id);
            var currentUser = await GetCurrentUserAsync();
            var current_user = currentUser.Id;
            if (response.IsSuccessStatusCode)
            {
                 data = await response.Content.ReadAsStringAsync();
                reviews = JsonConvert.DeserializeObject<List<Review>>(data);


            }
            ViewData["CurrentUser"] = current_user;
            return View("Index", reviews);
        }
        public async Task<IActionResult> Index2(int id)
        {
            List<Review> reviews = new List<Review>();
            string data = JsonConvert.SerializeObject(new { id_show = id });

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Review/Shows?id_show=" + id);
            var currentUser = await GetCurrentUserAsync();
            var current_user = currentUser.Id;
            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadAsStringAsync();
                reviews = JsonConvert.DeserializeObject<List<Review>>(data);


            }
            ViewData["CurrentUser"] = current_user;

            return View("Index2", reviews);
        }

        [HttpPost]
        public async Task<IActionResult> AddReviewMovie(int id, CreateReview review)
        {
            try
            {
                var id_user = Convert.ToInt32(HttpContext.Session.GetString("id_user"));

                // Set the id_movie in your review model
                review.Id_movie = id;
                review.Id_user = id_user;
                // Ensure 'review' is serialized to a valid JSON string
                string data = JsonConvert.SerializeObject(review);

                // Initialize StringContent with the JSON data
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                // Construct the request URI
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Review/movies", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new { id });
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddReviewShow(int id, CreateReview review)
        {
            try
            {
                var id_user = Convert.ToInt32(HttpContext.Session.GetString("id_user"));

                // Set the id_movie in your review model
                review.Id_show = id;
                review.Id_user = id_user;
                // Ensure 'review' is serialized to a valid JSON string
                string data = JsonConvert.SerializeObject(review);

                // Initialize StringContent with the JSON data
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                // Construct the request URI
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Review/shows", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index2", new { id });
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddReviewMovie()
        {
            return PartialView("AddReviewMovie");

        }
        [HttpGet]
        public async Task<IActionResult> AddReviewShow()
        {
            return PartialView("AddReviewShow");

        }

        [HttpPost]
        public async Task<IActionResult> EditReviewMovie(int id_movie,UpdateReview model)
        {
            try
            {
                String data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("IntraApiToken"));
                HttpResponseMessage response = await _httpClient.PutAsync(_httpClient.BaseAddress + "/Review/movies/?id=" + model.Id, content);
                if (response.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index", new {id=id_movie});

                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index", new { id = id_movie });

        }
        [HttpPost]
        public async Task<IActionResult> EditReviewShow(int id_show,UpdateReview model)
        {
            try
            {
                String data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("IntraApiToken"));
                HttpResponseMessage response = await _httpClient.PutAsync(_httpClient.BaseAddress + "/Review/shows/?id=" + model.Id, content);
                if (response.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index2", new { id = id_show });

                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index2");

            }
            return RedirectToAction("Index2", new { id = id_show });

        }
        [HttpGet]
        public async Task<IActionResult> GetMovieReviewForUpdate(int id)
        {
            Review review = new Review();

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Review/MovieReview/?id=" + id);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                review = JsonConvert.DeserializeObject<Review>(data);

                return View("EditReviewMovie", review);
            }
            else
            {
                return View("EditReviewMovie");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetShowReviewForUpdate(int id)
        {
            Review review = new Review();

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Review/ShowReview/?id=" + id);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                review = JsonConvert.DeserializeObject<Review>(data);

                return View("EditReviewShow", review);
            }
            else
            {
                return View("EditReviewShow");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReviewMovie(int id, int id_movie)
        {

            try
            {

                HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/Review/movies/{id}");



                if (response.IsSuccessStatusCode)
                {
                    //TempData["successMessage"] = "Movie Edited.";
                    return RedirectToAction("Index",new { id =id_movie});
                }

            }
            catch (Exception ex)
            {
                // Handle exception (log it, show error message, etc.)
            }
            return RedirectToAction("Index", new { id=id_movie });

        }

        [HttpPost]
        public async Task<IActionResult> DeleteReviewShow(int id, int id_show)
        {

            try
            {

                HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/Review/shows/{id}");



                if (response.IsSuccessStatusCode)
                {
                    //TempData["successMessage"] = "Movie Edited.";
                    return RedirectToAction("Index2", new { id = id_show });
                }

            }
            catch (Exception ex)
            {
                // Handle exception (log it, show error message, etc.)
            }
            return RedirectToAction("Index2", new { id = id_show });

        }
        private async Task<User> GetCurrentUserAsync()
        {
            User user = new User();
            var id_user = Convert.ToInt32(HttpContext.Session.GetString("id_user"));

            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("IntraApiToken"));

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/User/User?id=" + id_user).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<User>(data);
                //ViewBag.CurrentUser = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            }

            return user;


        }


    }
}
