using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookStore.Business.Models;
using BookStore.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Business.Services
{
    public interface IOrderService
    {
        Task<Order> GetOrderByUserIdAsync(string userId, CancellationToken cancellationToken);
        Task<Order> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken);
        Task<Order> CreateOrderAsync(string userId, CancellationToken cancellationToken);
        Task BuyItemAsync(string userId, OrderLine item, CancellationToken cancellationToken);
        Task RemoveItemAsync(long itemId);
    }

    public class OrderService : IOrderService
    {
        public OrderService(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                    .Where(x => x.UserId == userId)
                    .Include(x => x.OrderLines)
                    .ThenInclude(x => x.Book)
                    .ThenInclude(x => x.Author)
                    .Include(x => x.OrderLines)
                    .ThenInclude(x => x.Book.Category)
                    .Include(x => x.OrderLines)
                    .ThenInclude(x => x.Book.Publisher)
                    .FirstOrDefaultAsync(cancellationToken);

            if (order == null) return null;
            return order.FromEntity();
        }

        public async Task<Order> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                    .Where(x => x.Id == orderId)
                    .Include(x => x.OrderLines)
                    .ThenInclude(x => x.Book)
                    .ThenInclude(x => x.Author)
                    .Include(x => x.OrderLines)
                    .ThenInclude(x => x.Book.Category)
                    .Include(x => x.OrderLines)
                    .ThenInclude(x => x.Book.Publisher)
                    .FirstOrDefaultAsync(cancellationToken);

            if (order == null) return null;
            return order.FromEntity();
        }

        public async Task<Order> CreateOrderAsync(string userId, CancellationToken cancellationToken)
        {
            var order = new Persistence.Entities.Order
            {
                UserId = userId
            };

            _context.Orders.Add(order);
            var orderId = await _context.SaveChangesAsync(cancellationToken);
            return await GetOrderByIdAsync(orderId, cancellationToken);
        }

        public async Task BuyItemAsync(string userId, OrderLine item, CancellationToken cancellationToken)
        {
            var order = await GetOrderByUserIdAsync(userId, cancellationToken);
            if (order == null)
                order = await CreateOrderAsync(userId, cancellationToken);

            _context.OrderLines.Add(new Persistence.Entities.OrderLine
            {
                OrderId = order.Id,
                BookId = item.Book.Id,
                Quantity = item.Quantity,
                TotalPrice = item.Quantity * item.Book.Price
            });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveItemAsync(long itemId)
        {
            var orderLine = _context.OrderLines.Find(itemId);
            _context.OrderLines.Remove(orderLine);
            await _context.SaveChangesAsync();
        }

        private readonly BookStoreContext _context;
    }

    public static class MappingExtensions
    {
        public static Order FromEntity(this Persistence.Entities.Order order)
        {
            return new Order
            {
                Id = order.Id,
                TotalPrice = order.OrderLines.Sum(x => x.TotalPrice),
                TotalItemCount = order.OrderLines.Count,
                OrderLines = order.OrderLines.Select(x => new OrderLine
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    Book = new Book
                    {
                        Id = x.Book.Id,
                        Name = x.Book.Name,
                        Price = x.Book.Price,
                        Author = new Author
                        {
                            Id = x.Book.Author.Id,
                            Name = x.Book.Author.Name,
                            Surname = x.Book.Author.Surname
                        },
                        ISBN = x.Book.ISBN,
                        Images = x.Book.Images,
                        Category = new Category
                        {
                            Id = x.Book.Category.Id,
                            Name = x.Book.Category.Name
                        },
                        Publisher = new Publisher
                        {
                            Id = x.Book.Publisher.Id,
                            Name = x.Book.Publisher.Name
                        }
                    },
                    Quantity = x.Quantity,
                }).ToList()
            };
        }
    }
}
