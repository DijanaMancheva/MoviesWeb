using System.ComponentModel.DataAnnotations;

namespace MoviesWeb.Models.System
{
    public class LoginRequest
    {
        /// Внесено корисничко име
        [Required(ErrorMessage = "Полето {0} е задолжително")]
        [Display(Name = "Корисник")]        
        public string Username { get; set; }

        /// Внесена лозинка
        [Required(ErrorMessage = "Полето {0} е задолжително")]
        [Display(Name = "Лозинка")]
        public string Password { get; set; }
        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}
