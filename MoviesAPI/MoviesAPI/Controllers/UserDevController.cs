using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using MoviesAPI.Models.System;
using MoviesAPI.Repositories;
using Newtonsoft.Json;
using CodexIntraAPI.Authentication;
using MoviesAPI.Authentication;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDevController : ControllerBase
    {
        private readonly ILogger<UserDevController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IKorisnikService _korisnikService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public UserDevController(ILogger<UserDevController> logger, IUserRepository userRepository, IKorisnikService korisnikService, IJwtAuthManager jwtAuthManager)
        {
            _logger = logger;
            _userRepository = userRepository;
            _korisnikService = korisnikService;
            _jwtAuthManager = jwtAuthManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userRepository.GetUserAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUsers(int id)
        {
            var item = await _userRepository.GetUserAsync(id);
            if (item is null)
                return NotFound();
            else
                return Ok(item);
        }
        [HttpPost("Register")]
        public async Task<ActionResult> PostUser([FromForm] string values)
        {
            if (ModelState.IsValid)
            {
                CreateUser newZad = new CreateUser();
                JsonConvert.PopulateObject(values, newZad);

                var newId = await _userRepository.CreateUserAsync(newZad);

                return Ok(newId);
            }
            else return BadRequest();
        }
        [HttpPost]
        public async Task<ActionResult> PostUser(CreateUser newUser)
        {
            if (ModelState.IsValid)
            {

                var newId = await _userRepository.CreateUserAsync(newUser);
                return Ok(newId);
            }
            else return BadRequest();
        }
        [HttpPut]
        public async Task<ActionResult> PutUser([FromForm] int key, [FromForm] string values)
        {
            if (ModelState.IsValid)
            {
                var existingItem = await _userRepository.GetUserForUpdateAsync(key);

                if (existingItem is null)
                {
                    return NotFound();
                }
                JsonConvert.PopulateObject(values, existingItem);
                var numRecords = await _userRepository.UpdateUserAsync(key, existingItem);

                return Ok(numRecords);
            }
            else return BadRequest();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteUser([FromForm] int key)
        {
            var existingItem = await _userRepository.GetUserAsync(key);

            if (existingItem is null)
            {
                return NotFound();
            }

            try
            {
                var numRecords = await _userRepository.DeleteUserAsync(key);

                return Ok(numRecords);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User korisnik = _korisnikService.ProveriKorisnikAkreditivi(request.Username, request.Password);

            if (korisnik is null)
            {
                _logger.LogInformation($"Неуспешна најава во системот, корисничко име: [{request.Username}]");
                return Unauthorized();
            }


            return GenerateLoginResponse(korisnik);

        }
        protected ActionResult GenerateLoginResponse(User korisnik)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, korisnik.Username),
                new Claim("Korisnik_Id", korisnik.Id.ToString()),

            };





            var jwtResult = _jwtAuthManager.GenerateTokens(korisnik.Username, claims.ToArray(), DateTime.Now);
            _logger.LogInformation($"Корисникот [{korisnik.Username}] се најави во системот.");

            return Ok(new LoginResponse
            {
                Username = korisnik.Username,
                Korisnik = korisnik,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }
        [HttpGet("procedura")]
        public async Task<ActionResult> Get()
        {


            return Ok(await _userRepository.GetDashInfoAsync());
        }
    }
}
