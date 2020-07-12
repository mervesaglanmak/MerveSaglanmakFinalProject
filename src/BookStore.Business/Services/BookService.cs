using BookStore.Business.Models;
using BookStore.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Business.Services
{
    public interface IBookService
    {
        Task<Book> GetByIdAsync(long bookId, CancellationToken cancellationToken);
        Task<List<Book>> GetAllAsync(Expression<Func<Book, bool>> expression = null, CancellationToken cancellationToken = default);
        Task UpdateAsync(Book book, CancellationToken cancellationToken);
        Task DeleteAsync(Book book, CancellationToken cancellationToken);
        Task CreateAsync(Book book, CancellationToken cancellationToken);
        Task<List<Book>> GetFavoriteBooksByUserIdAsync(string userId, CancellationToken cancellationToken);
        Task MarkBookAsFavoriteByIdsAsync(long bookId, string userId, CancellationToken cancellationToken);
        Task RemoveFavoriteBookByIdsAsync(long bookId, string userId, CancellationToken cancellationToken);
    }

    public class BookService : IBookService
    {
        public BookService(BookStoreContext context)
        {
            _context = context;
            if (!context.Authors.Any())
                context.Authors.AddRange(BookStoreSeeder.GetAuthors());
            if (!context.Publishers.Any())
                context.Publishers.AddRange(BookStoreSeeder.GetPublishers());
            if (!context.Categories.Any())
                context.Categories.AddRange(BookStoreSeeder.GetCategories());
            if (!context.Books.Any())
                context.Books.AddRange(BookStoreSeeder.GetBooks());
            context.SaveChanges();
        }

        public Task<List<Book>> GetAllAsync(Expression<Func<Book, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            var books = _context.Books
                .Include(x => x.Author)
                .Include(x => x.Publisher)
                .Include(x => x.Category)
                .Select(x => new Book
            {
                Id = x.Id,
                Name = x.Name,
                Author = new Author
                {
                    Id = x.Author.Id,
                    Name = x.Author.Name,
                    Surname = x.Author.Surname,
                    DateOfBirth = x.Author.DateOfBirth,
                    Description = x.Author.Description,
                    ProfileImage = x.Author.ProfileImage,
                },
                Category = new Category
                {
                    Id = x.Category.Id,
                    Name = x.Category.Name,
                    ParentId = x.Category.ParentId
                },
                Images = x.Images,
                ISBN = x.ISBN,
                Price = x.Price,
                Rating = x.Rating,
                Publisher = new Publisher
                {
                    Id = x.Publisher.Id,
                    Name = x.Publisher.Name
                },
            });

            books = expression != null ? books.Where(expression) : books;
            return books.ToListAsync(cancellationToken);
        }

        public Task UpdateAsync(Book book, CancellationToken cancellationToken)
        {
            var entity = GetBookById(book.Id);

            entity.Name = book.Name;
            entity.ISBN = book.ISBN;
            entity.Images = book.Images;
            entity.Price = book.Price;
            entity.PublisherId = book.Publisher.Id;
            entity.AuthorId = book.Author.Id;
            entity.CategoryId = book.Category.Id;
            entity.Rating = book.Rating;
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(Book book, CancellationToken cancellationToken)
        {
            var entity = GetBookById(book.Id);
            _context.Books.Remove(entity);
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task<Book> GetByIdAsync(long bookId, CancellationToken cancellationToken)
        {
            var book = GetBookById(bookId);
            return Task.Run(() =>
            {
                return new Book
                {
                    Id = book.Id,
                    Name = book.Name,
                    Author = new Author
                    {
                        Id = book.Author.Id,
                        Name = book.Author.Name,
                        Surname = book.Author.Surname,
                        DateOfBirth = book.Author.DateOfBirth,
                        Description = book.Author.Description,
                        ProfileImage = book.Author.ProfileImage,
                    },
                    Category = new Category
                    {
                        Id = book.Category.Id,
                        Name = book.Category.Name,
                        ParentId = book.Category.ParentId
                    },
                    Images = book.Images,
                    ISBN = book.ISBN,
                    Price = book.Price,
                    Rating = book.Rating,
                    Publisher = new Publisher
                    {
                        Id = book.Publisher.Id,
                        Name = book.Publisher.Name
                    },
                };
            }, cancellationToken);
        }

        public Task CreateAsync(Book book, CancellationToken cancellationToken)
        {
            var entity = new Persistence.Entities.Book
            {
                Name = book.Name,
                Author = new Persistence.Entities.Author
                {
                    Id = book.Author.Id,
                    Name = book.Author.Name,
                    Surname = book.Author.Surname,
                    DateOfBirth = book.Author.DateOfBirth,
                    Description = book.Author.Description,
                    ProfileImage = book.Author.ProfileImage,
                },
                Category = new Persistence.Entities.Category
                {
                    Id = book.Category.Id,
                    Name = book.Category.Name,
                    ParentId = book.Category.ParentId
                },
                Images = book.Images,
                ISBN = book.ISBN,
                Price = book.Price,
                Publisher = new Persistence.Entities.Publisher
                {
                    Id = book.Publisher.Id,
                    Name = book.Publisher.Name
                },
                CreatedOn = DateTime.UtcNow
            };
            _context.Books.Add(entity);
            return _context.SaveChangesAsync(cancellationToken);
        }

        private Persistence.Entities.Book GetBookById(long bookId)
        {
            var entity = _context.Books.Where(x => x.Id == bookId)
                .Include(x => x.Author)
                .Include(x => x.Publisher)
                .Include(x => x.Category)
                .FirstOrDefault();

            if (entity == null)
                throw new ArgumentNullException($"Book could not be found with id {bookId}");
            return entity;
        }

        public Task<List<Book>> GetFavoriteBooksByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return _context.FavoriteBooks.Where(x => x.UserId == userId)
                .Include(x => x.Book)
                .ThenInclude(x => x.Author)
                .Include(x => x.Book)
                .ThenInclude(x => x.Category)
                .Include(x => x.Book)
                .ThenInclude(x => x.Publisher)
                .Select(x => new Book
                {
                    Id = x.Book.Id,
                    Name = x.Book.Name,
                    Author = new Author
                    {
                        Id = x.Book.Author.Id,
                        Name = x.Book.Author.Name,
                        Surname = x.Book.Author.Surname,
                        DateOfBirth = x.Book.Author.DateOfBirth,
                        Description = x.Book.Author.Description,
                        ProfileImage = x.Book.Author.ProfileImage,
                    },
                    Category = new Category
                    {
                        Id = x.Book.Category.Id,
                        Name = x.Book.Category.Name,
                        ParentId = x.Book.Category.ParentId
                    },
                    Images = x.Book.Images,
                    ISBN = x.Book.ISBN,
                    Price = x.Book.Price,
                    Rating = x.Book.Rating,
                    Publisher = new Publisher
                    {
                        Id = x.Book.Publisher.Id,
                        Name = x.Book.Publisher.Name
                    },
                }).ToListAsync(cancellationToken);
        }

        public async Task MarkBookAsFavoriteByIdsAsync(long bookId, string userId, CancellationToken cancellationToken)
        {
            _context.FavoriteBooks.Add(new Persistence.Entities.FavoriteBook
            {
                BookId = bookId,
                UserId = userId
            });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveFavoriteBookByIdsAsync(long bookId, string userId, CancellationToken cancellationToken)
        {
            var favoriteBook = _context.FavoriteBooks.Where(x => x.BookId == bookId && x.UserId == userId).FirstOrDefault();
            _context.FavoriteBooks.Remove(favoriteBook);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private readonly BookStoreContext _context;
    }
}
