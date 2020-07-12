using System.Collections.Generic;

namespace BookStore.Persistence.Entities
{
    public class Order : Entity
    {
        public string UserId { get; set; }
        public ICollection<OrderLine> OrderLines { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscount { get; set; }
    }
}
