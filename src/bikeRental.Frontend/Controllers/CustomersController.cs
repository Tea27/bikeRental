using bikeRental.Application.Models.Station;
using bikeRental.Application;
using bikeRental.Application.Models.User;
using bikeRental.Application.Services;
using bikeRental.Application.Services.Impl;
using bikeRental.Core.Entities;
using bikeRental.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace bikeRental.Frontend.Controllers;
public class CustomersController : Controller
{
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    public CustomersController(IUserService userService, UserManager<ApplicationUser> userManager) {
        _userService = userService;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize(Roles = ("Administrator"))]
    public async Task<IActionResult> Index(string currentCategory, string currentFilter, string searchString, int? pageNumber)
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
        var customers = await _userService.GetAllUsers();


        return View("/Pages/Customers/Index.cshtml", PaginatedList<UserModel>.Create(customers, pageNumber ?? 1, pageSize));
    }
}
