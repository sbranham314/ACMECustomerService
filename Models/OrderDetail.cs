using System.ComponentModel.DataAnnotations;

namespace ACMECorpCustomerService.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        [Required]
        public int OrderId {  get; set; }

        [Required]
        public int ItemId { get; set; }

        public Item item { get; set; }

        public int Quantity { get; set; }

        public Order Order { get; set; }

    }
}
