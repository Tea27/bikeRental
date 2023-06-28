using bikeRental.Application;
using bikeRental.Application.Models.Station;
using bikeRental.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bikeRental.Frontend.Controllers;
public class StationsController : Controller
{
    private readonly IStationService _stationService;
    private readonly IBicycleService _bicycleService;

    public StationsController(IStationService stationService, IBicycleService bicycleService)
    {
        _stationService = stationService;
        _bicycleService = bicycleService;
    }

    [HttpGet]
    public IActionResult Index(string searchString, string sortOrder, int? pageNumber)
    {
        if (searchString != null)
        {
            pageNumber = 1;
        }

        ViewData["searchString"] = searchString;
        ViewData["sortOrder"] = sortOrder;
        int pageSize = 5;

        var stations = _stationService.CheckSwitch(searchString, sortOrder);

        return View("/Pages/Stations/Index.cshtml", PaginatedList<StationResponse>.Create(stations, pageNumber ?? 1, pageSize));
    }


    [Authorize(Roles = ("Administrator"))]
    public IActionResult Create()
    {
        return View("/Pages/Stations/Create.cshtml");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StationModel stationModel)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _stationService.AddAsync(stationModel);
            }
        }
        catch (DbUpdateException ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            ModelState.AddModelError("", "Unable to save changes. " + ex);
        }
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = ("Administrator"))]
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
            return BadRequest();

        var station = await _stationService.GetByIdAsync(id);

        if (station == null)
        {
            return BadRequest();
        }

        return View("/Pages/Stations/Edit.cshtml", station);
    }

    [HttpPost, ActionName("Edit")]
    [Authorize(Roles = ("Administrator"))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(StationModel stationModel)
    {
        if (stationModel == null)
        {
            return BadRequest();
        }

        try
        {
            await _stationService.UpdateAsync(stationModel);
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Unable to save changes. " + ex);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = ("Administrator"))]
    public async Task<IActionResult> Delete(Guid? id, bool? saveChangesError = false)
    {

        if (id == null)
            return BadRequest();

        var station = await _stationService.GetByIdAsync(id);

        if (station == null)
        {
            return BadRequest();
        }
        
        if (saveChangesError.GetValueOrDefault())
        {
            ViewData["ErrorMessage"] = _stationService.SaveError(id);
        }

        return View("/Pages/Stations/Delete.cshtml", station);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = ("Administrator"))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
       
        try
        {
            if (_stationService.HasOrders(id))
            {
                throw new DbUpdateException();
            }
            await _stationService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true }) ;
        }
    }
    [HttpGet]
    public async Task<IActionResult> Disable(Guid id)
    {
        await _stationService.DisableBicycles(id);
        return RedirectToAction(nameof(Index));
    }


}

