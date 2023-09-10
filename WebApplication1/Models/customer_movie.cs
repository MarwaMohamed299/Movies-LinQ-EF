namespace WebApplication1.Models
{
    public class customer_movie
    {
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
        public int CustomerId { get; set; }
        public customer? Customer { get; set; }
        public DateTime TimeRented { get; set; }
        public DateTime DueDate { get; set; }

    }
}
