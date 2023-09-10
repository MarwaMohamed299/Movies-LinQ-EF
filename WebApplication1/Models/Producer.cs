namespace WebApplication1.Models

{
    public class Producer
    {

        public int ProducerId { get; set; } 
        public string CompanyName { get; set; }
        public string Country { get; set; }

        public List <Movie>? Movies { get; set; }

    }

}
