using System;

namespace BookStore.Business.Models
{
    public class Author
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Description { get; set; }
        public string ProfileImage { get; set; }
    }
}
