namespace MoviesAPI.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Id_movie { get; set; } // Foreign Key to Movie
        public int Id_user { get; set; } // Foreign Key to User
        public int Id_show { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }
    }
    public class CreateReview
    {
        public int? Id_movie { get; set; } // Foreign Key to Movie
        public int? Id_user { get; set; } // Foreign Key to User
        public int? Id_show { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? Rating { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }


    }
    public class UpdateReview
    {
        public int Id { get; set; }
        public int Id_movie { get; set; } // Foreign Key to Movie
        public int Id_user { get; set; } // Foreign Key to User
        public int Id_show { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public int Rating { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }

    }
    public class DeleteReview
    {
        public int Id { get; set; }
        public int Id_movie { get; set; } // Foreign Key to Movie
        public int Id_user { get; set; } // Foreign Key to User
        public int Id_show { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public int Rating { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }

    }
}
