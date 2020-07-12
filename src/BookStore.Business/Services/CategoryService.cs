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
    public interface ICategoryService
    {
        Task<Category> GetByIdAsync(long categoryId, CancellationToken cancellationToken);
        Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>> expression = null, CancellationToken cancellationToken = default);
        Task UpdateAsync(Category category, CancellationToken cancellationToken);
        Task DeleteAsync(Category category, CancellationToken cancellationToken);
        Task CreateAsync(Category category, CancellationToken cancellationToken);
    }

    public class CategoryService : ICategoryService
    {
        public CategoryService(BookStoreContext context)
        {
            _context = context;
        }

        public Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            var categories = _context.Categories.Select(x => new Category
            {
                Id = x.Id,
                Name = x.Name,
                Order = x.Order,
                ParentId = x.ParentId
            });

            categories = expression != null ? categories.Where(expression) : categories;
            return categories.ToListAsync(cancellationToken);
        }

        public Task UpdateAsync(Category category, CancellationToken cancellationToken)
        {
            var entity = GetCategoryById(category.Id);
            entity.Name = category.Name;
            entity.Order = category.Order;
            entity.ParentId = category.ParentId;
            entity.ModifiedOn = DateTime.UtcNow;
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(Category category, CancellationToken cancellationToken)
        {
            var entity = GetCategoryById(category.Id);
            _context.Categories.Remove(entity);
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task<Category> GetByIdAsync(long categoryId, CancellationToken cancellationToken)
        {
            var category = GetCategoryById(categoryId);
            return Task.Run(() =>
            {
                return new Category
                {
                    Id = category.Id,
                    Name = category.Name,
                    Order = category.Order,
                    ParentId = category.ParentId
                };
            }, cancellationToken);
        }

        public Task CreateAsync(Category category, CancellationToken cancellationToken)
        {
            var entity = new Persistence.Entities.Category
            {
                Name = category.Name,
                Order = category.Order,
                ParentId = category.ParentId
            };
            _context.Categories.Add(entity);
            return _context.SaveChangesAsync(cancellationToken);
        }

        private Persistence.Entities.Category GetCategoryById(long categoryId)
        {
            var entity = _context.Categories.Find(categoryId);

            if (entity == null)
                throw new ArgumentNullException($"Category could not be found with id {categoryId}");
            return entity;
        }

        private readonly BookStoreContext _context;
    }
}
