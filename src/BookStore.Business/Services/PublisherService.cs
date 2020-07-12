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
    public interface IPublisherService
    {
        Task<Publisher> GetByIdAsync(long publisherId, CancellationToken cancellationToken);
        Task<List<Publisher>> GetAllAsync(Expression<Func<Publisher, bool>> expression = null, CancellationToken cancellationToken = default);
        Task UpdateAsync(Publisher publisher, CancellationToken cancellationToken);
        Task DeleteAsync(Publisher publisher, CancellationToken cancellationToken);
        Task CreateAsync(Publisher publisher, CancellationToken cancellationToken);
    }

    public class PublisherService : IPublisherService
    {
        public PublisherService(BookStoreContext context)
        {
            _context = context;
        }

        public Task<List<Publisher>> GetAllAsync(Expression<Func<Publisher, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            var categories = _context.Publishers.Select(x => new Publisher
            {
                Id = x.Id,
                Name = x.Name,
            });

            categories = expression != null ? categories.Where(expression) : categories;
            return categories.ToListAsync(cancellationToken);
        }

        public Task UpdateAsync(Publisher publisher, CancellationToken cancellationToken)
        {
            var entity = GetPublisherById(publisher.Id);
            entity.Name = publisher.Name;
            entity.ModifiedOn = DateTime.UtcNow;
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(Publisher publisher, CancellationToken cancellationToken)
        {
            var entity = GetPublisherById(publisher.Id);
            _context.Publishers.Remove(entity);
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task<Publisher> GetByIdAsync(long publisherId, CancellationToken cancellationToken)
        {
            var publisher = GetPublisherById(publisherId);
            return Task.Run(() =>
            {
                return new Publisher
                {
                    Id = publisher.Id,
                    Name = publisher.Name,
                };
            }, cancellationToken);
        }

        public Task CreateAsync(Publisher publisher, CancellationToken cancellationToken)
        {
            var entity = new Persistence.Entities.Publisher
            {
                Name = publisher.Name,
            };
            _context.Publishers.Add(entity);
            return _context.SaveChangesAsync(cancellationToken);
        }

        private Persistence.Entities.Publisher GetPublisherById(long publisherId)
        {
            var entity = _context.Publishers.Find(publisherId);

            if (entity == null)
                throw new ArgumentNullException($"Publisher could not be found with id {publisherId}");
            return entity;
        }

        private readonly BookStoreContext _context;
    }
}
