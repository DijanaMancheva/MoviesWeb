namespace MoviesAPI.Models
{
    public class Show
    {        
        public int Id { get; set; }

        public DateTime Release_date { get; set; }   
        public int Seasons { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public string? Director { get; set; }        
        public string? Description { get; set; }
        public byte[]? Poster_image { get; set; }
        public int Price { get; set; }
        public bool IsAvailable { get; set; }

        public string? Trailer { get; set; }
        public string? ImageBase64
        {
            get
            {
                if (Poster_image != null && Poster_image.Length > 0)
                {
                    return "data:image/jpeg;base64," + Convert.ToBase64String(Poster_image);
                    // Change "image/jpeg" to the appropriate MIME type if your image format is different.
                }
                return string.Empty;
            }
        }



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
        public int Price { get; set; }
        public bool IsAvailable { get; set; }
        public string? Trailer { get; set; }

    }
    public class CreateShow
    {
       // public int Id { get; set; }

        public DateTime Release_date { get; set; }
        public int Seasons { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public string? Director { get; set; }
        public string? Description { get; set; }
        public byte[]? Poster_image { get; set; }
        public int Price { get; set; }
        public bool IsAvailable { get; set; }

        public string? Trailer { get; set; }




    }
    public class CreateShowInCard
    {
        public int Id_show { get; set; }
        public int Id_user { get; set; }
    }

}
