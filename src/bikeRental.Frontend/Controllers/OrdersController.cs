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

namespace bikeRental.Frontend.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IBicycleService _bicycleService;

        public OrdersController(IOrderService orderService, IUserService userService, IBicycleService bicycleService)
        {
            _orderService = orderService;
            _userService = userService;
            _bicycleService = bicycleService;
        }

        [HttpGet]
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

            int pageSize = 5;

            var orders = await _orderService.GetAllAsync();
            
            orders = _orderService.SortingSelection(orders, sortOrder);
            orders = _orderService.SearchSelectionAsync(orders, searchDate);
                      
            return View("/Pages/Orders/Index.cshtml", PaginatedList<OrderResponse>.Create(orders, pageNumber ?? 1, pageSize));
        }

        [Authorize]
        public async Task<IActionResult> Create(Guid? bicycleId, Guid stationId)
        {
            var userID = Guid.Parse(User.Identity.GetUserId());
            if (bicycleId == null || stationId == null)
            {
                return NotFound();
            }
    
            var order = new OrderModel { RentalStartTime = DateTime.Now,
                                         RentalPrice = 0,
                                         Customer = await _userService.GetByIdAsync(userID),
                                         Bicycle = await _bicycleService.GetByIdAsync(bicycleId, stationId)};
            Console.WriteLine("-.....rentalstart.." + order.RentalStartTime);
            Console.WriteLine("-.....rentalprice.." + order.RentalPrice);
            Console.WriteLine("-.....userId.." + userID);
            Console.WriteLine("-.....bicycle.." + order.Bicycle.Description);
            Console.WriteLine("-.....customer.." + order.Customer.UserName);
            
            return View("/Pages/Orders/Create.cshtml", order);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(OrderModel orderModel)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    TimeSpan ts = orderModel.RentalEndTime - orderModel.RentalStartTime;
                    decimal hours = Convert.ToDecimal(ts.Ticks);
                    var price = orderModel.Bicycle.Price * hours;
                    orderModel.RentalPrice = price;
                    Console.WriteLine("-.....ts.." + orderModel.RentalStartTime);
                    Console.WriteLine("-.....hours.." + hours);
                    Console.WriteLine("-.....price.." + price);
                    Console.WriteLine("-.....bicycle.." + orderModel.Bicycle.Id);
                    Console.WriteLine("-.....customer.." + orderModel.Customer.UserName);
                    await _orderService.AddAsync(orderModel, orderModel.Customer.Id, orderModel.Bicycle.Id);
                }
                else
                {
                    Console.WriteLine("-.....nije valid..");
                }
                
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("-.....error..");
                System.Diagnostics.Debug.WriteLine(ex);
                ModelState.AddModelError("", "Unable to save changes. " + ex);
            }
            return RedirectToAction(nameof(Index));
        }
        
      
    }
}
