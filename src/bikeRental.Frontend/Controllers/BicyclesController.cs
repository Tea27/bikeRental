using bikeRental.Application;
using Microsoft.AspNetCore.Mvc;
using bikeRental.Application.Models.Bicycle;
using bikeRental.Application.Services;
using Microsoft.EntityFrameworkCore;
using bikeRental.Core.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;


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
        public async Task<IActionResult> GetByStation(Guid Id, string searchString, string filterString, string sortOrder, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }

            int pageSize = 6;

            var station = await _stationService.GetByIdAsync(Id);

            ViewData["searchString"] = searchString;
            ViewData["filterString"] = filterString;
            ViewData["sortOrder"] = sortOrder;
            ViewData["id"] = station.Id;
            ViewData["address"] = station.Address;

            var bicycles = _bicycleService.CheckSwitch(filterString, searchString, sortOrder, Id);

            return View("/Pages/Bicycles/BicyclesOnStation.cshtml", PaginatedList<BicycleModel>.Create(bicycles, pageNumber ?? 1, pageSize));

        }

        [HttpGet]
        [Authorize(Roles = ("Administrator"))]
        public async Task<IActionResult> Delete(Guid? id, Guid stationId, string cname, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ViewData["cname"] = cname;
            var bicycle = await _bicycleService.GetByIdAsync(id);

            if (bicycle == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                   "Bicycle cannot be removed, it has order relation: ";                  
            }

            return View("/Pages/Bicycles/Delete.cshtml", bicycle);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = ("Administrator"))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Guid stationId, string cname)
        {
            var bicycle = await _bicycleService.GetByIdAsyncIncludeOrders(id);

            if (bicycle == null)
            {
                return RedirectToAction(nameof(Delete));
            }

            try
            {
                if (bicycle.Orders.Any())
                {
                    throw new DbUpdateException();
                }
                await _bicycleService.Delete(id);
                return (cname == "GetByStation") ? RedirectToAction(cname, new { id = stationId }) : RedirectToAction(cname);
            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction(nameof(Delete), new { id = id, cname = cname, saveChangesError = true });
            }
        }


        public IActionResult Index(string searchString, string filterString, string sortOrder, int? pageNumber)
        {

            if (searchString != null)
            {
                pageNumber = 1;
            }


            ViewData["searchString"] = searchString;
            ViewData["filterString"] = filterString;
            ViewData["sortOrder"] = sortOrder;

            int pageSize = 6;

            var bicycles = _bicycleService.CheckSwitch(filterString, searchString, sortOrder);

            return View("/Pages/Bicycles/Index.cshtml", PaginatedList<BicycleModel>.Create(bicycles, pageNumber ?? 1, pageSize));

        }

        [Authorize(Roles = ("Administrator"))]
        public async Task<IActionResult> Create(Guid? stationId)
        {
            if (stationId == null)
            {
                return BadRequest();
            }

            var bicycle = new BicycleModel { Station = await _stationService.GetByIdAsync(stationId) };
            return View("/Pages/Bicycles/Create.cshtml", bicycle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BicycleModel bicycleModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _bicycleService.AddAsync(bicycleModel, bicycleModel.Station.Id);
                }
            }
            catch (DbUpdateException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                ModelState.AddModelError("", "Unable to save changes. " + ex);
            }
            return RedirectToAction(nameof(GetByStation), new { id = bicycleModel.Station.Id });
        }

        [Authorize(Roles = ("Administrator"))]
        public async Task<IActionResult> Edit(Guid? id, Guid stationId, string cname)
        {

            if (id == null)
            {
                return BadRequest();
            }

            ViewData["cname"] = cname;

            var bicycle = await _bicycleService.GetByIdAsync(id);

            return View("/Pages/Bicycles/Edit.cshtml", bicycle);
        }

        [HttpPost, ActionName("Edit")]
        [Authorize(Roles = ("Administrator"))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(BicycleModel bicycleModel, string cname)
        {
            if (bicycleModel == null)
            {
                return BadRequest();
            }

            try
            {
                await _bicycleService.UpdateAsync(bicycleModel);
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " + ex);
            }

            return (cname == "GetByStation") ? RedirectToAction(cname, new { id = bicycleModel.Station.Id }) : RedirectToAction(cname);
        }

        [HttpGet]
        public async Task<IActionResult> Disable(Guid id, string cname)
        {
            var bicycle = await _bicycleService.GetByIdAsync(id);
            bicycle.Status = BikeStatus.Disabled;
            await _bicycleService.UpdateAsync(bicycle);

            return (cname == "GetByStation") ? RedirectToAction(cname, new { id = bicycle.Station.Id }) : RedirectToAction(cname);
        }


    }
}
