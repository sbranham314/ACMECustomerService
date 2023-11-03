using System.ComponentModel.DataAnnotations;

namespace ACMECorpCustomerService.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; }

        public List<OrderDetail> OrderDetail { get; set; }
    }
}
