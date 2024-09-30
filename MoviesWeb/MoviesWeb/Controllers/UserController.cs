using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoviesWeb.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
using MoviesWeb.Models.System;
using MoviesWeb.Services;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.CompilerServices;

namespace MoviesWeb.Controllers
{
    public class UserController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44367/api");
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;//
        private readonly CxSettings _cxSettings;//
        private readonly ILogger<MovieController> _logger;
        //private readonly CxSettings _cxSettings;
        private string id;
        public UserController(IHttpContextAccessor context,IUserService userService, IOptions<CxSettings> cxSettings)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
            _userService = userService;
            _cxSettings = cxSettings.Value;
            id = context.HttpContext.Session.GetString("id");

        }

        public async Task<IActionResult> Index()
        {
            List<User> users = new List<User>();

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/User");
            var currentUser = await GetCurrentUserAsync();
            var isAdmin = currentUser.Admin;
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<User>>(data);


            }
            ViewData["IsAdmin"] = isAdmin;
            //ViewBag.CurrentUser = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name).Value;
            return View("Index", users);
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
        [HttpPost]
        public async Task<IActionResult> AddUser(User model)
        {

            try
            {


                String data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("IntraApiToken"));
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/User", content);

                if (response.IsSuccessStatusCode)
                {
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
        public async Task<IActionResult> AddUser()
        {
            return PartialView("AddUser");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {

            try
            {
                


                HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + "/User/Delete/?id=" + id);



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

        [HttpPost]
        public async Task<IActionResult> EditUser(UpdateUser model)
        {

            try
            {
                String data = JsonConvert.SerializeObject(model);

                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync(_httpClient.BaseAddress + "/User/Update/?id=" + model.Id, content);
                if (response.IsSuccessStatusCode)
                {
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
        public async Task<IActionResult> GetUserForUpdate(int id)
        {
            User user = new User();
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("IntraApiToken"));

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/User/User/?id=" + id);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<User>(data);

                return View("EditUser", user);
            }
            else
            {
                return View("EditUser");
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            var savedUsername = Request.Cookies["LoginCookieUsername"];
            var savedPassword = Request.Cookies["LoginCookiePassword"];

            var loginRequest = new LoginRequest
            {
                Username = savedUsername,
                Password=savedPassword
            };

            return View(loginRequest);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest, bool rememberMe)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // Log or inspect the error messages
                }
                return View(loginRequest);
            }

            // Verify user credentials using the user service
            LoginResponse loginResponse = await _userService.ProveriKorisnikAkreditivi(loginRequest);

            if (loginResponse != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, loginResponse.Korisnik.Name.ToString()),
            new Claim(ClaimTypes.Name, loginResponse.Korisnik.Name),
            new Claim("Korisnik_Id", loginResponse.Korisnik.Id.ToString()),
            new Claim(ClaimTypes.Name,loginResponse.Korisnik.Username,loginResponse.Korisnik.Password)
        };

                // Create claims identity
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Create claims principal
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe, // Set IsPersistent based on "Remember me" checkbox
                };
                //Response.Cookies.Append("LoginCookie",model.Username)
                //var role = loginResponse.Korisnik.Role ; // Default to "User" if Role is null

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                if (loginRequest.RememberMe)
                {
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddDays(30),
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    };
                    Response.Cookies.Append("LoginCookieUsername", loginRequest.Username, cookieOptions);
                    Response.Cookies.Append("LoginCookiePassword", loginRequest.Password, cookieOptions);

                }
                // Set the session
                HttpContext.Session.SetString("id_user", loginResponse.Korisnik.Id.ToString());
                HttpContext.Session.SetString("UserName", loginResponse.Korisnik.Name.ToString());
                //HttpContext.Session.SetString("Role", loginResponse.Korisnik.Role.ToString());
                HttpContext.Session.SetString("Admin", loginResponse.Korisnik.Admin.ToString());




                return RedirectToAction("HomePage", "Home");
            }

            ModelState.AddModelError("", "Погрешно корисничко име или лозинка!");
            return View(loginRequest);


        }
        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            // Simulate sending a reset link or token.
            // You should replace this with actual logic to handle password reset functionality.

            // Assume that the email is valid and the reset link is sent successfully.

            // Redirect to the confirmation page
            return View();
        }

        [HttpGet]
        public async Task< IActionResult> ResetPassword()
        {
           
            return View();
        }
        [HttpGet]
        public  IActionResult GetUserForUpdate2()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetUserForUpdate2(User user)
        {
            string data=JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(data,Encoding.UTF8,"application/json");
           
            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/User/Email" ,content);

            if (response.IsSuccessStatusCode)
            {
                string data2 = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<User>(data2);

                return View("ResetPassword", user);
            }
            else
            {
                return View("ResetPassword");
            }
        }
        [HttpGet]
        public IActionResult GetUserForChange()
        {

            return View();
        }

       


        [HttpPost]
        public async Task<IActionResult> GetUserForChange(User user)
        {

            try
            {
                String data = JsonConvert.SerializeObject(user);

                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response =  _httpClient.PutAsync(_httpClient.BaseAddress + "/User/Reset?id=" + user.Id, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                //TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
            return RedirectToAction("ResetPassowrd");

        }
        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(UpdateUser model)
        //{


        //        try
        //        {
        //                    String data = JsonConvert.SerializeObject(model);

        //                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

        //                    HttpResponseMessage response = await _httpClient.PutAsync(_httpClient.BaseAddress + "/User/Reset/" + model.Id, content);

        //                    if (response.IsSuccessStatusCode)
        //            {
        //                return RedirectToAction("Login");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["errorMessage"] = ex.Message;
        //        }


        //    return RedirectToAction("ResetPassword");
        //}

        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation(User user)
        {
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return PartialView("Register");
        }
        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {

            try
            {


                String data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("IntraApiToken"));
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/User/", content);

                if (response.IsSuccessStatusCode)
                {

                    //string responseData = await response.Content.ReadAsStringAsync();
                    //var registeredUser = JsonConvert.DeserializeObject<User>(responseData);
                    return RedirectToAction("Login");
                }
                else
                {
                    // Handle unsuccessful registration response (e.g., show error message)
                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                //TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View(model);

        }
       
        [HttpPost]
        
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", new {ShowLogout=true});
        }

       
    }
}
