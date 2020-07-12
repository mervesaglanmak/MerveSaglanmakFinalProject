using BookStore.Business.Models;
using BookStore.Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStore.Application.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IBookService _bookService;
        public FavoritesController(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var viewModel = new FavoriteBooksViewModel();
            viewModel.Books = await _bookService.GetFavoriteBooksByUserIdAsync(userId, default);
            return View(viewModel);
        }

        public async Task<JsonResult> MarkBookAsFavorite(long bookId)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _bookService.MarkBookAsFavoriteByIdsAsync(bookId, userId, default);
            return new JsonResult(true);
        }

        public async Task<JsonResult> UnMarkFavoriteBook(long bookId)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _bookService.RemoveFavoriteBookByIdsAsync(bookId, userId, default);
            return new JsonResult(true);
        }
    }

    public class FavoriteBooksViewModel
    {
        public List<Book> Books { get; set; }
    }
}
