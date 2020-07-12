namespace BookStore.Business.Models
{
    public class Book
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ISBN { get; set; }

        public decimal Price { get; set; }

        public string Images { get; set; }

        public decimal Rating { get; set; }

        public Author Author { get; set; }

        public Category Category { get; set; }

        public Publisher Publisher { get; set; }
    }

    public class BookFilterCriteria
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public long[] CategoryIds { get; set; }
        public long[] AuthorIds { get; set; }
        public string Name { get; set; }
        public bool Ascending { get; set; }
    }
}
