namespace MoviesWeb.Models
{
    public class Buy
    {
        public int Id { get; set; }
        public int Id_user { get; set; }
        public int Id_movie { get; set; }
    }
    public class UpdateBuy
    {
        public int Id { get; set; }

        public int Id_user { get; set; }
        public int Id_movie { get; set; }

    }
    public class DeleteBuy
    {
        public int Id { get; set; }

        public int Id_user { get; set; }
        public int Id_movie { get; set; }
        public decimal Amount { get; set; }
        public DateTime Release_date { get; set; }
        public int Duration { get; set; }

        public string Title { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public byte[] Poster_image { get; set; }
        public int Price { get; set; }
        public bool IsAvailable { get; set; }
    }
    public class CreateBuy
    {
        public decimal Amount { get; set; }
        public DateTime Release_date { get; set; }
        public int Duration { get; set; }

        public string Title { get; set; }
        public int Id { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public byte[] Poster_image { get; set; }
        public int Price { get; set; }
        public bool IsAvailable { get; set; }

    }
}
