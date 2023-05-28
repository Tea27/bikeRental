using bikeRental.Application.Models.Station;
using bikeRental.Application;
using bikeRental.Application.Services;
using bikeRental.Application.Services.Impl;
using Microsoft.AspNetCore.Mvc;

namespace bikeRental.Frontend.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string currentCategory, string sortOrder, string currentFilter, DateTime searchDate, int? pageNumber)
        {
            if (searchDate != null)
            {
                pageNumber = 1;
            }

            int pageSize = 5;

            var orders = await _orderService.GetAllAsync();

            orders = _orderService.SortingSelection(orders, sortOrder);

            orders = _orderService.SearchSelectionAsync(orders, searchDate);

            return View("/Pages/Stations/Index.cshtml", PaginatedList<StationResponse>.Create(orders, pageNumber ?? 1, pageSize));
        }
    }
}
