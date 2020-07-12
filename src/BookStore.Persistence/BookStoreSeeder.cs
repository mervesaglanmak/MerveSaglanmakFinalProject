using BookStore.Persistence.Entities;

namespace BookStore.Persistence
{
    public static class BookStoreSeeder
    {
        public static Author[] GetAuthors()
        {
            return new[]
            {
                new Author
                {
                    Id = 1,
                    Name = "Sabahattin Ali",
                },
                new Author
                {
                    Id = 2,
                    Name = "George Orwell",
                },
                new Author
                {
                    Id = 3,
                    Name = "Yuval Noah Harari",
                },
                new Author
                {
                    Id = 4,
                    Name = "Stefan Zweig",
                },
            };
        }

        public static Publisher[] GetPublishers()
        {
            return new[]
            {
                new Publisher()
                {
                    Id = 1,
                    Name = "Yapi Kredi",
                },
                new Publisher()
                {
                    Id = 2,
                    Name = "Can",
                },
                new Publisher()
                {
                    Id = 3,
                    Name = "Harper",
                },
                new Publisher()
                {
                    Id = 4,
                    Name = "Domingo"
                },
            };
        }

        public static Category[] GetCategories()
        {
            return new[]
            {
                new Category
                {
                    Id = 1,
                    Name = "Ask",
                },
                new Category
                {
                    Id = 2,
                    Name = "Politik",
                },
                new Category
                {
                    Id = 3,
                    Name = "Antropoloji",
                },
                new Category
                {
                    Id = 4,
                    Name = "Bilim-Kurgu",
                },
            };
        }

        public static Book[] GetBooks()
        {
            return new[]
            {
                new Book
                {
                    Id = 1,
                    Name = "Kurk Mantolu Madonna",
                    ISBN = "9786053424765",
                    Price = 3.5m,
                    Images = "https://i.dr.com.tr/cache/500x400-0/originals/0001795788001-1.jpg",
                    AuthorId = 1,
                    CategoryId = 1,
                    PublisherId = 1,
                    Rating = 5,
                },
                new Book
                {
                    Id = 2,
                    Name = "Hayvan Çiftliği",
                    ISBN = "9789750719387",
                    Price = 13.50m,
                    Images = "https://i.dr.com.tr/cache/500x400-0/originals/0000000105409-1.jpg",
                    AuthorId = 2,
                    CategoryId = 2,
                    PublisherId = 2,
                    Rating = 3,
                },
                new Book
                {
                    Id = 3,
                    Name = "Sapiens",
                    ISBN = "9786055029357",
                    Price = 27.00m,
                    Images = "https://i.dr.com.tr/cache/500x400-0/originals/0000000633872-1.jpg",
                    AuthorId = 3,
                    CategoryId = 3,
                    PublisherId = 3,
                    Rating = 4,
                },
                new Book
                {
                    Id = 4,
                    Name = "Homo Deus",
                    ISBN = "9786055029630",
                    Price = 27.00m,
                    Images = "https://i.dr.com.tr/cache/500x400-0/originals/0000000726638-1.jpg",
                    AuthorId = 3,
                    CategoryId = 3,
                    PublisherId = 3,
                    Rating = 1
                },
                new Book
                {
                   Id = 5,
                   Name = "Kizil",
                   ISBN = "9786057703200",
                   Price = 7.50m,
                   Images = "https://i.dr.com.tr/cache/500x400-0/originals/0001862253001-1.jpg",
                   AuthorId = 4,
                   CategoryId = 4,
                   PublisherId = 1,
                   Rating = 1,
                }
            };
        }
    }
}