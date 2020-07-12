using System;

namespace BookStore.Persistence.Entities
{
    public class Author : Entity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Description { get; set; }
        public string ProfileImage { get; set; }
    }
}
