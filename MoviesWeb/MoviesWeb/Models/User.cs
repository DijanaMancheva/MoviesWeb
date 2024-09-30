using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MoviesWeb.Models
{
    public class User
    {
        public bool Active { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }
        public int Id { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
        public bool Admin { get; set; } // Use IsAdmin instead of Role
        public string? Email { get; set; }

    }
    public class DeleteUser
    {
        public bool Active { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public bool Admin { get; set; } // Use IsAdmin instead of Role
        public string Email { get; set; }

    }
    public class UpdateUser
    {
        public bool Active { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }
        public int Id { get; set; }
      
        public bool Admin { get; set; } // Use IsAdmin instead of Role
        public string? Email { get; set; }
    }

}

