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
    public interface IAuthorService
    {
        Task<Author> GetByIdAsync(long authorId, CancellationToken cancellationToken);
        Task<List<Author>> GetAllAsync(Expression<Func<Author, bool>> expression = null, CancellationToken cancellationToken = default);
        Task UpdateAsync(Author author, CancellationToken cancellationToken);
        Task DeleteAsync(Author author, CancellationToken cancellationToken);
        Task CreateAsync(Author author, CancellationToken cancellationToken);
    }

    public class AuthorService : IAuthorService
    {
        public AuthorService(BookStoreContext context)
        {
            _context = context;
        }

        public Task<List<Author>> GetAllAsync(Expression<Func<Author, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            var authors = _context.Authors.Select(x => new Author
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                DateOfBirth = x.DateOfBirth,
                Description = x.Description,
                ProfileImage = x.ProfileImage
            });

            authors = expression != null ? authors.Where(expression) : authors;
            return authors.ToListAsync(cancellationToken);
        }

        public Task UpdateAsync(Author author, CancellationToken cancellationToken)
        {
            var entity = GetAuthorById(author.Id);
            
            entity.Name = author.Name;
            entity.Surname = author.Surname;
            entity.DateOfBirth = author.DateOfBirth;
            entity.Description = author.Description;
            entity.ProfileImage = author.ProfileImage;
            entity.ModifiedOn = DateTime.UtcNow;
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(Author author, CancellationToken cancellationToken)
        {
            var entity = GetAuthorById(author.Id);
            _context.Authors.Remove(entity);
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task<Author> GetByIdAsync(long authorId, CancellationToken cancellationToken)
        {
            var author = GetAuthorById(authorId);
            return Task.Run(() =>
            {
                return new Author
                {
                    Id = author.Id,
                    Name = author.Name,
                    Surname = author.Surname,
                    DateOfBirth = author.DateOfBirth,
                    Description = author.Description,
                    ProfileImage = author.ProfileImage
                };
            }, cancellationToken);
        }

        public Task CreateAsync(Author author, CancellationToken cancellationToken)
        {
            var entity = new Persistence.Entities.Author
            {
                Name = author.Name,
                Surname = author.Surname,
                DateOfBirth = author.DateOfBirth,
                Description = author.Description,
                ProfileImage = author.ProfileImage,
                CreatedOn = DateTime.UtcNow
            };
            _context.Authors.Add(entity);
            return _context.SaveChangesAsync(cancellationToken);
        }

        private Persistence.Entities.Author GetAuthorById(long authorId)
        {
            var entity = _context.Authors.Find(authorId);

            if (entity == null)
                throw new ArgumentNullException($"Author could not be found with id {authorId}");
            return entity;
        }

        private readonly BookStoreContext _context;
    }
}
