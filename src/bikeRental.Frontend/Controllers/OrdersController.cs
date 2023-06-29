using bikeRental.Application;
using bikeRental.Application.Services;
using Microsoft.AspNetCore.Mvc;
using bikeRental.Application.Models.Order;
using Microsoft.AspNetCore.Authorization;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity;
using AutoMapper;
using bikeRental.Core.Enums;

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
        public IActionResult Index(string sortOrder, DateTime searchDateFrom, DateTime searchDateTo, int? pageNumber)
        {
            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            ViewData["SearchDateFrom"] =searchDateFrom.ToString("yyyy-MM-dd");
            ViewData["SearchDateTo"] =searchDateTo.ToString("yyyy-MM-dd");
            ViewData["SortOrder"] = sortOrder;
            int pageSize = 6;

            var orders = _orderService.GetAll();

            orders = _orderService.SortingSelection(orders, sortOrder);
            if (searchDateFrom != DateTime.MinValue || searchDateTo != DateTime.MinValue)
            {
                orders = _orderService.SearchSelection(orders, searchDateFrom, searchDateTo);
            }
            
            return View("/Pages/Orders/Index.cshtml", PaginatedList<OrderModel>.Create(orders, pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserIndex(string sortOrder, DateTime searchDateFrom, DateTime searchDateTo, int? pageNumber)
        {
            var userID = Guid.Parse(User.Identity.GetUserId());

            ViewData["SortOrder"] = sortOrder;
            ViewData["SearchDateFrom"] = searchDateFrom.ToString("yyyy-MM-dd");
            ViewData["SearchDateTo"] = searchDateTo.ToString("yyyy-MM-dd");

            int pageSize = 6;

            var orders = await _orderService.GetByCustomer(userID);
            orders = _orderService.SortingSelection(orders, sortOrder);


            if (searchDateFrom != DateTime.MinValue || searchDateTo != DateTime.MinValue)
            {
                orders = _orderService.SearchSelection(orders, searchDateFrom, searchDateTo);
            }

            return View("/Pages/Orders/UserIndex.cshtml", PaginatedList<OrderModel>.Create(orders, pageNumber ?? 1, pageSize));
        }

        [Authorize]
        public async Task<IActionResult> Create(Guid? bicycleId)
        {
            var userID = Guid.Parse(User.Identity.GetUserId());
            if (bicycleId == null )
            {
                return BadRequest();
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
            try
            {
                if (ModelState.IsValid)
                {
                    var bicycle = await _bicycleService.GetByIdAsync(orderModel.Bicycle.Id);
                    bicycle.Status = BikeStatus.InUse;
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
            ViewData["Stations"] = _stationService.GetAll();
       
            var userID = Guid.Parse(User.Identity.GetUserId());
            if (orderId == null)
            {
                return BadRequest();
            }
            var order = await _orderService.GetByIdAsync(orderId, userID, bicycleId);
            var station = await _stationService.GetByIdAsync(order.Bicycle.Station.Id);
            order.Bicycle.Station = station;
            order.RentalEndTime = DateTime.Now;
            return View("/Pages/Orders/Finish.cshtml", order);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Finish(OrderModel orderModel, Guid stationId)
        {
            var user = await _userService.GetByIdAsync(Guid.Parse(User.Identity.GetUserId()));      

            if(user.Id != orderModel.Customer.Id)
            {
                return BadRequest();
            }  
            
            try
            {
                if (ModelState.IsValid)
                {
                    await _orderService.FinishOrder(orderModel, stationId);
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
    }
}
