using System.Security.Claims;
using CodexIntraAPI.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Authentication;
using MoviesAPI.Models;
using MoviesAPI.Models.System;
using MoviesAPI.Repositories;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IKorisnikService _korisnikService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository,IKorisnikService korisnikService,IJwtAuthManager jwtAuthManager)
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
        [HttpGet("User")]
        public async Task<ActionResult> GetUsers(int id)
        {
            var item = await _userRepository.GetUserAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            else return Ok(item);
        }
        [HttpPost("Email")]
        public async Task<ActionResult<User>> GetUsersByEmail([FromBody] User user)
        {
            var item = await _userRepository.GetUserByEmailAsync(user.Email);
            if (item == null)
            {
                return NotFound();
            }
            else return Ok(item);
        }
        //[HttpPost("Password")]
        //public async Task<ActionResult<User>> GetUsersByPassword(int id)
        //{
        //    var item = await _userRepository.GetUserByPasswordAsync(id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }
        //    else return Ok(item);
        //}
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
        [HttpPut("Update")]
        public async Task<ActionResult> PutUser(int id,User user)
        {
            if (ModelState.IsValid)
            {
                var existingItem = await _userRepository.GetUserForUpdateAsync(id);
                if (existingItem == null)
                {
                    return NotFound();
                }
                
                var numRecords = await _userRepository.UpdateUserAsync(id, user);

                return Ok(numRecords);
            }
            else return BadRequest();
        }
        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var existingItem = await _userRepository.GetUserAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            try
            {
                var numRecords = await _userRepository.DeleteUserAsync(id);
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
        [HttpPut("Reset")]
        public async Task<ActionResult> ResetPassword(int id, User updUser)
        {
            if (ModelState.IsValid)
            {
                var existingItem = await _userRepository.GetUserForUpdateAsync(id);
                if (existingItem == null)
                {
                    return NotFound();
                }

                var numRecords = await _userRepository.UpdatePasswordAsync(id, updUser);

                return Ok(numRecords);
            }
            else return BadRequest();
        }


    }
}
