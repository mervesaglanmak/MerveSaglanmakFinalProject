using BookStore.Business.Models;
using BookStore.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Business.Services
{
    public interface IFilterService
    {
        Task<List<Book>> FilterBooksAsync(BookFilterModel filterModel, CancellationToken cancellationToken);
    }

    public class FilterService : IFilterService
    {
        private readonly BookStoreContext _context;
        public FilterService(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> FilterBooksAsync(BookFilterModel filterModel, CancellationToken cancellationToken)
        {
            var allBooks = await _context.Books.AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Include(x => x.Publisher)
                .ToListAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(filterModel.BookName))
            {
                allBooks = allBooks.Where(x => x.Name.ToLower().Contains(filterModel.BookName.ToLower())).ToList();
            }

            if (filterModel.MinPrice.HasValue)
            {
                allBooks = allBooks.Where(x => x.Price >= filterModel.MinPrice).ToList();
            }

            if (filterModel.MaxPrice.HasValue)
            {
                allBooks = allBooks.Where(x => x.Price <= filterModel.MaxPrice).ToList();
            }

            if (filterModel.PublisherIds != null && filterModel.PublisherIds.Any())
            {
                allBooks = allBooks.Where(x => filterModel.PublisherIds.Contains(x.PublisherId)).ToList();
            }

            if(filterModel.AuthorsIds != null && filterModel.AuthorsIds.Any())
            {
                allBooks = allBooks.Where(x => filterModel.AuthorsIds.Contains(x.AuthorId)).ToList();
            }

            if (filterModel.CategoryIds != null && filterModel.CategoryIds.Any())
            {
                allBooks = allBooks.Where(x => filterModel.CategoryIds.Contains(x.CategoryId)).ToList();
            }

            return allBooks.Select(x => new Book
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
            }).ToList();
        }
    }
}
