using Microsoft.AspNetCore.Mvc.Rendering;
using MoviesWeb.Models;

namespace MoviesWeb.Models
{
    public class Movie

    {
        public decimal? Amount { get; set; }
        public DateTime Release_date { get; set; }
        public int Duration { get; set; }

        public string? Title { get; set; }
        public int Id { get; set; }
        public string? Genre { get; set; }
        public string? Director { get; set; }
        public string? Description { get; set; }
        public byte[]? Poster_image { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int Price { get; set; }

        public bool IsAvailable { get; set; }
        public string? Trailer {  get; set; }




    }
    public class UpdateMovie

    {
        public decimal Amount { get; set; }
        public DateTime Release_date { get; set; }
        public int Duration { get; set; }

        public string? Title { get; set; }
        public int Id { get; set; }
        public string? Genre { get; set; }
        public string? Director { get; set; }
        public string? Description { get; set; }
        public byte[]? Poster_image { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int Price { get; set; }

        public bool IsAvailable { get; set; }
        public string? Trailer { get; set; }


    }
    public class DeleteMovie

    {
        //public decimal Amount { get; set; }
        public DateTime Release_date { get; set; }
        public int Duration { get; set; }

        public string Title { get; set; }
        public int Id { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public byte[] Poster_image { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int Price { get; set; }

        public bool IsAvailable { get; set; }
        public string? Trailer { get; set; }
       


    }
    public class DeleteMovieFromCard

    {
        //public decimal Amount { get; set; }
        public DateTime Release_date { get; set; }
        public int Duration { get; set; }

        public string Title { get; set; }
        public int Id { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public byte[] Poster_image { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int Price { get; set; }

        public bool IsAvailable { get; set; }
        public string? Trailer { get; set; }




    }
}
