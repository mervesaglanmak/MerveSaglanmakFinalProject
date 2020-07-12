using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Persistence.Entities
{
    public class OrderLine : Entity
    {
        
        public long BookId { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalDiscount { get; set; }
     
        public long OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

    }
}
