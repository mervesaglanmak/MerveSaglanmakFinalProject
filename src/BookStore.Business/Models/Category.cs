using System;

namespace BookStore.Business.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public long ParentId { get; set; }
    }
}
