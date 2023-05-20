using bikeRental.Application.Models.Station;
using bikeRental.Application;
using bikeRental.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using bikeRental.Application.Models.Bicycle;
using bikeRental.Application.Services;
using bikeRental.Application.Services.Impl;
using AutoMapper;

namespace bikeRental.Frontend.Controllers
{
    public class BicyclesController : Controller
    {
        
        private readonly IBicycleService _bicycleService;
        private readonly IStationService _stationService;
        private readonly IMapper _mapper;
        public BicyclesController(IBicycleService bicycleService, IStationService stationService, IMapper mapper)
        {
            _bicycleService = bicycleService;
            _mapper = mapper;
            _stationService = stationService;
        }


        [HttpGet]
        public async Task<IActionResult> GetByStation(Guid? Id, string currentCategory, string currentFilter, string searchString, int? pageNumber)
        {

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentCategory"] = currentCategory;
            ViewData["CurrentFilter"] = searchString;
            int pageSize = 5;
            
            var bicycles = await _bicycleService.GetByStation(Id);

            bicycles = _bicycleService.SearchSelection(bicycles, searchString);

            return View("/Pages/Bicycles/BicyclesOnStation.cshtml", PaginatedList<BicycleModel>.Create(bicycles, pageNumber ?? 1, pageSize));

        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
