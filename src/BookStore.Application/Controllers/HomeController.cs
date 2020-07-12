using Microsoft.AspNetCore.Mvc;
using BookStore.Business.Services;
using System.Collections.Generic;
using BookStore.Business.Models;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Claims;

namespace BookStore.Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IFilterService _filterService;
        private readonly IOrderService _orderService;

        public HomeController(IBookService bookService, IOrderService orderService, IFilterService filterService)
        {
            _bookService = bookService;
            _filterService = filterService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index(decimal? minPrice, decimal? maxPrice, long[] publisherIds, long[] categoryIds, long[] authorIds, CancellationToken cancellationToken)
        {
            var viewModel = new BooksViewModel();
            var books = await _bookService.GetAllAsync(cancellationToken: cancellationToken);
            viewModel.Books = books;

            viewModel.MinPrice = books.OrderBy(x => x.Price).First().Price;
            viewModel.MaxPrice = books.OrderByDescending(x => x.Price).First().Price;
            viewModel.Categories = books.Select(x => x.Category).GroupBy(x => x.Id).Select(x => x.First()).ToList();
            viewModel.Authors = books.Select(x => x.Author).GroupBy(x => x.Id).Select(x => x.First()).ToList();
            viewModel.Publishers = books.Select(x => x.Publisher).GroupBy(x => x.Id).Select(x => x.First()).ToList();

            if (minPrice.HasValue || maxPrice.HasValue || publisherIds.Any() || categoryIds.Any() || authorIds.Any())
            {
                var filterModel = new BookFilterModel
                {
                    MaxPrice = maxPrice.Value,
                    MinPrice = minPrice.Value,
                    PublisherIds = publisherIds,
                    AuthorsIds = authorIds,
                    CategoryIds = categoryIds,
                };
                viewModel.Books = await _filterService.FilterBooksAsync(filterModel, cancellationToken);
                viewModel.FilterModel = filterModel;
            }

            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            viewModel.OrderCount = (await _orderService.GetOrderByUserIdAsync(userId, cancellationToken))?.OrderLines?.Count() ?? 0;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> AddToCart(long bookId)
        {
            var book = await _bookService.GetByIdAsync(bookId, default);
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _orderService.BuyItemAsync(userId, new OrderLine
            {
                Book = book,
                Quantity = 1
            }, default);
            return new JsonResult(true);
        }
    }
    
    public class BooksViewModel
    {
        public int OrderCount { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public List<Category> Categories { get; set; }
        public List<Author> Authors { get; set; }
        public List<Publisher> Publishers { get; set; }
        public BookFilterModel FilterModel { get; set; }
        public IEnumerable<Book> Books { get; internal set; }
    }
}
