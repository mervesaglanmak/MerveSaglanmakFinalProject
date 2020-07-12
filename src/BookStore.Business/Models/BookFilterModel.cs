namespace BookStore.Business.Models
{
    public class BookFilterModel
    {
        public string BookName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public long[] CategoryIds { get; set; }
        public long[] PublisherIds { get; set; }
        public long[] AuthorsIds { get; set; }
    }
}
