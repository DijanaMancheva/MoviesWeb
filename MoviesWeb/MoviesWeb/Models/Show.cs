using MoviesWeb.Models;

namespace MoviesWeb.Models
{
    public class Show
    {        
        public int Id { get; set; }

        public DateTime Release_date { get; set; }   
        public int Seasons { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }        
        public string Description { get; set; }
        public byte[] Poster_image { get; set; }
        public IFormFile? ImageFile { get; set; }

        public int Price { get; set; }
        public bool IsAvailable { get; set; }
        public string? Trailer { get; set; }






    }
    public class UpdateShow
    {
        public int Id { get; set; }

        public DateTime Release_date { get; set; }
        public int Seasons { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public byte[] Poster_image { get; set; }
        public IFormFile? ImageFile { get; set; }

        public int Price { get; set; }
        public bool IsAvailable { get; set; }
        public string? Trailer { get; set; }

    }
    public class DeleteShow
    {
        public int Id { get; set; }

        public DateTime Release_date { get; set; }
        public int Seasons { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public byte[] Poster_image { get; set; }
        public IFormFile? ImageFile { get; set; }

        public int Price { get; set; }
        public bool IsAvailable { get; set; }
        public string? Trailer { get; set; }

    }
    public class DeleteShowFromCard
    {
        public int Id { get; set; }

        public DateTime Release_date { get; set; }
        public int Seasons { get; set; }
        public string Title { get; set; }
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
