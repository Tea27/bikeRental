using bikeRental.Application.Models.Station;
using bikeRental.Application;
using bikeRental.Application.Services;
using bikeRental.Application.Services.Impl;
using Microsoft.AspNetCore.Mvc;
using bikeRental.Application.Models.Order;
using Microsoft.AspNetCore.Authorization;
using System.Data.Entity.Infrastructure;

namespace bikeRental.Frontend.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            /*if (searchDate != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchDate = currentFilter;
            }

            ViewData["CurrentCategory"] = currentCategory;
            ViewData["CurrentFilter"] = searchDate;

            int pageSize = 5;

            var orders = await _orderService.GetAllAsync();

            orders = _orderService.SortingSelection(orders, sortOrder);

            orders = _orderService.SearchSelectionAsync(orders, searchDate);
            */
            return View("/Pages/Orders/Index.cshtml");
        }

        [Authorize(Roles = ("Administrator"))]
        public IActionResult Create()
        {
            return View("/Pages/Orders/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderModel orderModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _orderService.AddAsync(orderModel);
                }
            }
            catch (DbUpdateException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                ModelState.AddModelError("", "Unable to save changes. " + ex);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
