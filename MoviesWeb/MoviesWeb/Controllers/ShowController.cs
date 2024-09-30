using System.Text;
using Microsoft.AspNetCore.Mvc;
using MoviesWeb.Models;
using MoviesWeb.Models;
using Newtonsoft.Json;

namespace MoviesWeb.Controllers
{
    public class ShowController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44367/api");
        private string data;
        private readonly HttpClient _httpClient;
       
        public ShowController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        [HttpGet]
        public async Task<IActionResult> Search(string searchString)
        {
            List<Show> shows = new List<Show>();

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Show/Search?searchString=" + searchString);


            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                shows = JsonConvert.DeserializeObject<List<Show>>(data);
            }

            return View("Index", shows);

        }
        private readonly ILogger<ShowController> _logger;

       
        [HttpPost]
        public async Task<IActionResult> AddShow(Show model)
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
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Show", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
            return View();

        }
        [HttpGet]
        public async Task<IActionResult> AddShow()
        {
            return PartialView("AddShow");
        }
        [HttpPost]
        public async Task<IActionResult> EditShow(UpdateShow model)
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
                HttpResponseMessage response = await _httpClient.PutAsync(_httpClient.BaseAddress + "/Show/" + model.Id, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> GetShowForUpdate(int id)
        {
            Show show = new Show();

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Show/" + id);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                show = JsonConvert.DeserializeObject<Show>(data);

                return View("EditShow", show);
            }
            else
            {
                return View("EditShow");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteShow(int id)
        {

            try
            {

                HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + "/Show?id=" + id);



                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                // Handle exception (log it, show error message, etc.)
            }
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Index(string genre)
        {
            List<Show> shows = new List<Show>();

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Show");
            var currentUser = await GetCurrentUserAsync();
            var isAdmin = currentUser.Admin;

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                shows = JsonConvert.DeserializeObject<List<Show>>(data);

            }

            if (!string.IsNullOrEmpty(genre))
            {
                shows = shows.Where(m => m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            ViewData["IsAdmin"] = isAdmin;
            return View("Index", shows);
            return View("Index", shows);
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
        public async Task<IActionResult> GetShowsInCard()
        {
            List<Show> shows = new List<Show>();
            //ViewBag.CurrentUser = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var id_user = Convert.ToInt32(HttpContext.Session.GetString("id_user"));

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Show/Status?id_user=" + id_user);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                shows = JsonConvert.DeserializeObject<List<Show>>(data);
            }


            return View("Buy", shows);
        }
        [HttpPost]
        public async Task<IActionResult> Buy(int id)
        {
            try
            {
                var id_user = Convert.ToInt32(HttpContext.Session.GetString("id_user"));

                // Ensure 'data' is a valid JSON string
                string data = JsonConvert.SerializeObject(new { id_show = id, id_user });

                // Initialize StringContent with the JSON data
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                // Construct the request URI
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Show/Status?id_show=" + id + "&id_user=" + id_user, content);

                // Make the POST request with content

                if (response.IsSuccessStatusCode)
                {

                    return RedirectToAction("Buy");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = "The show is already in your card." });

                    
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
            List<Show> shows = new List<Show>();

            var id_user = Convert.ToInt32(HttpContext.Session.GetString("id_user"));

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Show/Status?id_user=" + id_user);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                shows = JsonConvert.DeserializeObject<List<Show>>(data);

            }
            else
            {
                TempData["errorMessage"] = "Failed to retrive shows";
            }



            return View("Buy");
        }
        [HttpGet, ActionName("DeleteShowConfirmed")]
        public async Task<IActionResult> DeleteShowFromCard(int id)
        {

            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + "/Show/Status?id=" + id);
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Show details deleted";
                   
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
                HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + "/Show/Clear?id_user=" + id_user);

                // Call API to clear all movies from the card for the current user

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Payment successful and card cleared!";
                    return RedirectToAction("Index", "Show"); // Redirect to a relevant page after clearing the card
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
        public async Task<IActionResult> GetShowsInProfile()
        {
            List<Show> shows = new List<Show>();
            //ViewBag.CurrentUser = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var id_user = Convert.ToInt32(HttpContext.Session.GetString("id_user"));

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Show/CLear?id_user=" + id_user);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                shows = JsonConvert.DeserializeObject<List<Show>>(data);
            }


            return View("Profile", shows);
        }

    }
}
