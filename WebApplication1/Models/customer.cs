using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class customer


    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public List<customer_movie> CustomersOfMovies { get; set; }
        public List<CustomerFeedBack> Feedbacks { get; set; }

    }
}
