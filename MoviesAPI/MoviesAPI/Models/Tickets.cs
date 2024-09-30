﻿namespace MoviesAPI.Models
{
    public class Tickets
    {        
        public decimal Amount { get; set; }
        public DateTime Watch_movie { get; set; }
        public int User_id { get; set; }
        public int Movie_id { get; set; }
        public int Id { get; set; }


    }
    public class UpdateTickets
    {
        public decimal Amount { get; set; }
        public DateTime Watch_movie { get; set; }
        public int User_id { get; set; }
        public int Movie_id { get; set; }
        //public int Id { get; set; }
    }
    public class CreateTicket
    {
        public decimal Amount { get; set; }
        public DateTime Watch_movie { get; set; }
        public int User_id { get; set; }
        public int Movie_id { get; set; }
        public int Id { get; set; }

    }
}
