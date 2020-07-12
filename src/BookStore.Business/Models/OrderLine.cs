namespace BookStore.Business.Models
{
    public class OrderLine
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
