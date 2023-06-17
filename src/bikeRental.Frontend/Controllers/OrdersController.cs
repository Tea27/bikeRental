using bikeRental.Application.Models.Station;
using bikeRental.Application;
using bikeRental.Application.Services;
using bikeRental.Application.Services.Impl;
using Microsoft.AspNetCore.Mvc;
using bikeRental.Application.Models.Order;
using Microsoft.AspNetCore.Authorization;
using System.Data.Entity.Infrastructure;
using bikeRental.Core.Entities;
using Microsoft.AspNet.Identity;
using bikeRental.Core.Identity;
using bikeRental.Application.Models.User;
using System.Xml.Linq;
using bikeRental.Application.Models.Bicycle;
using AutoMapper;

namespace bikeRental.Frontend.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IBicycleService _bicycleService;
        private readonly IStationService _stationService;
        private readonly IMapper _mapper;


        public OrdersController(IOrderService orderService, IUserService userService, IBicycleService bicycleService, IStationService stationService, IMapper mapper)
        {
            _orderService = orderService;
            _userService = userService;
            _bicycleService = bicycleService;
            _stationService = stationService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = ("Administrator"))]
        public async Task<IActionResult> Index(string currentCategory, string sortOrder, DateTime currentFilter, DateTime searchDate, int? pageNumber)
        {
            if (!searchDate.Equals(new DateTime(0001, 01, 01, 00, 00, 00)))
            {
                pageNumber = 1;
            }
            else
            {
                searchDate = currentFilter;
            }

            ViewData["CurrentCategory"] = currentCategory;
            ViewData["CurrentFilter"] = searchDate;
            ViewData["SortOrder"] = sortOrder;
            int pageSize = 4;

            var orders = await _orderService.GetAllAsync();

            orders = _orderService.SortingSelection(orders, sortOrder);
            orders = _orderService.SearchSelectionAsync(orders, searchDate);

            return View("/Pages/Orders/Index.cshtml", PaginatedList<OrderResponse>.Create(orders, pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserIndex(string currentCategory, string sortOrder, int? pageNumber)
        {
            var userID = Guid.Parse(User.Identity.GetUserId());

            ViewData["CurrentCategory"] = currentCategory;
            ViewData["SortOrder"] = sortOrder;

            int pageSize = 4;

            var orders = await _orderService.GetAllAsync();
            List<OrderResponse> userOrders = new List<OrderResponse>();

            var stations = _stationService.GetAll();

            foreach (OrderResponse order in orders)
            {
                if (order.Customer.Id == userID)
                {
                    foreach (StationResponse s in stations)
                    {
                        var bicycles = s.Bicycles;
                        foreach (BicycleModel b in bicycles)
                        {
                            if (order.Bicycle.Id == b.Id)
                            {
                                var station = await _stationService.GetByIdAsync(s.Id);
                                order.Bicycle.Station = station;
                                userOrders.Add(order);
                            }
                        }
                    }
                }
            }

            IEnumerable<OrderResponse> response = userOrders.AsEnumerable();
            response = _orderService.SortingSelection(response, sortOrder);

            return View("/Pages/Orders/UserIndex.cshtml", PaginatedList<OrderResponse>.Create(response, pageNumber ?? 1, pageSize));
        }

        [Authorize]
        public async Task<IActionResult> Create(Guid? bicycleId, Guid stationId)
        {
            var userID = Guid.Parse(User.Identity.GetUserId());
            if (bicycleId == null || stationId == null)
            {
                return NotFound();
            }

            var order = new OrderModel
            {
                RentalStartTime = DateTime.Now,
                RentalEndTime = DateTime.Now,
                RentalPrice = 0,
                Customer = await _userService.GetByIdAsync(userID),
                Bicycle = await _bicycleService.GetByIdAsync(bicycleId)
            };
            return View("/Pages/Orders/Create.cshtml", order);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(OrderModel orderModel)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            try
            {
                if (ModelState.IsValid)
                {
                    var bicycle = await _bicycleService.GetByIdAsync(orderModel.Bicycle.Id);
                    bicycle.IsAvailable = false;
                    await _bicycleService.UpdateAsync(bicycle);
                    await _orderService.AddAsync(orderModel, orderModel.Customer.Id, orderModel.Bicycle.Id);
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("-.....error..");
                System.Diagnostics.Debug.WriteLine(ex);
                ModelState.AddModelError("", "Unable to save changes. " + ex);
            }
            return RedirectToAction(nameof(UserIndex));
        }

        [Authorize]
        public async Task<IActionResult> Finish(Guid? orderId, Guid bicycleId, Guid stationId)
        {
            var userID = Guid.Parse(User.Identity.GetUserId());
            if (orderId == null || bicycleId == null || stationId == null)
            {
                return NotFound();
            }
            var order = await _orderService.GetByIdAsync(orderId, userID, bicycleId);
            var station = await _stationService.GetByIdAsync(stationId);
            order.Bicycle.Station = station;
            order.RentalEndTime = DateTime.Now;
            return View("/Pages/Orders/Finish.cshtml", order);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Finish(OrderModel orderModel)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            try
            {
                if (ModelState.IsValid)
                {
                    var diffOfDates = (orderModel.RentalEndTime).Subtract(orderModel.RentalStartTime);
                    var days = diffOfDates.Days;
                    var hours = diffOfDates.Hours;
                    var minutes = diffOfDates.Minutes;
                    var total = minutes + (hours * 60) + (days * 24 * 60);
                    orderModel.RentalPrice = Math.Ceiling(Decimal.Divide(total, 30)) * orderModel.Bicycle.Price;

                    if (orderModel.RentalPrice != 0)
                    {
                        var bicycle = await _bicycleService.GetByIdAsync(orderModel.Bicycle.Id);
                        bicycle.IsAvailable = true;
                        await _bicycleService.UpdateAsync(bicycle);
                        await _orderService.UpdateAsync(orderModel);
                    }
                    else
                    {
                        orderModel.RentalPrice = orderModel.Bicycle.Price;
                        var bicycle = await _bicycleService.GetByIdAsync(orderModel.Bicycle.Id);
                        bicycle.IsAvailable = true;
                        await _bicycleService.UpdateAsync(bicycle);
                        await _orderService.UpdateAsync(orderModel);
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("-.....error..");
                System.Diagnostics.Debug.WriteLine(ex);
                ModelState.AddModelError("", "Unable to save changes. " + ex);
            }
            return RedirectToAction(nameof(UserIndex));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id, Guid bicycleId, Guid customerId, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderService.GetByIdAsync(id, customerId, bicycleId);

            if (order == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View("/Pages/Orders/Delete.cshtml", order);
        }


        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Guid bicycleId, Guid customerId)
        {
            var order = await _orderService.GetByIdAsync(id, customerId, bicycleId);
            var loggedUserId = Guid.Parse(User.Identity.GetUserId());
            var user = await _userService.GetByIdAsync(loggedUserId);
            if (order == null)
            {
                return RedirectToAction(nameof(Delete));
            }

            try
            {
                await _orderService.Delete(id);
                if (user.Role == Core.Enums.Role.Administrator && loggedUserId != order.Customer.Id)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(UserIndex));


            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }


    }
}
