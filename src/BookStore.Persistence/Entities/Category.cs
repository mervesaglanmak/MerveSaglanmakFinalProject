using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Persistence.Entities
{
    [Table("Category")]
    public class Category : Entity
    {
        public Category()
        {
            CreatedOn = DateTime.UtcNow;
        }

        public string Name { get; set; }
        public long ParentId { get; set; }
        public int Order { get; set; }
    }
}
