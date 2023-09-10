namespace WebApplication1.Models
{
    public class Movie
    {

        public int MovieId { get; set; }
        public int ProducerId { get; set; }
        public Producer? Producer { get; set; }
        public string Rating  { get; set; }
        public int Duration { get; set; }
        public string  Title { get; set; }
        public List<customer_movie> MoviesOfCustomers { get; set; }

    }
}
