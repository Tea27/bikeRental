using bikeRental.Application.Models;
using bikeRental.Application.Services;
using bikeRental.Frontend.Filters;
using GoogleMaps.LocationServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Diagnostics;

namespace bikeRental.Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStationService _stationService;
        public HomeController(ILogger<HomeController> logger, IStationService stationService)
        {
            _logger = logger;
            _stationService = stationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var stations = _stationService.GetAll();
            ViewBag.Markers = _stationService.GetAddressess();
            return View("/Pages/Home/Index.cshtml", stations);
        }

        [CustomAuthorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
