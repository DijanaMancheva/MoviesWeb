using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models
{
    public class User
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
    public class UpdateUser
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
    public class CreateUser
    {
        public bool Active { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }
        public bool Admin { get; set; } // Use IsAdmin instead of Role
        public string? Email { get; set; }
    }
}
