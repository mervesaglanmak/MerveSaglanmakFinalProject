using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Persistence.Entities
{
    public class FavoriteBook : Entity
    {
        public string UserId { get; set; }

  
        public long BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }
}
