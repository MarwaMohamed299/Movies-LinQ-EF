using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class CustomerFeedBack
    {

        public int Id { get; set; }
        public string? Comments { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public customer Customer { get; set; }



    }
}
