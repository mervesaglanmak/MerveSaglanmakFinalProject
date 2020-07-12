using BookStore.Business.Models;
using BookStore.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStore.Application.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var viewModel = new OrdersViewModel();
            viewModel.Order = await _orderService.GetOrderByUserIdAsync(userId, default);
            return View(viewModel);
        }

        public async Task<JsonResult> RemoveItem(long id)
        {
            await _orderService.RemoveItemAsync(id);
            return new JsonResult(true);
        }
    }

    public class OrdersViewModel
    {
        public Order Order { get; set; }
    }
}
