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
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Web;
using System;
using System.Security.Cryptography;
using System.Data.Entity.Validation;
using Microsoft.AspNetCore.Authentication;

namespace bikeRental.Frontend.Controllers;
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UsersController(IMapper mapper, IUserService userService, UserManager<ApplicationUser> userManager)
    {
        _mapper = mapper;
        _userService = userService;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize(Roles = ("Administrator"))]
    public async Task<IActionResult> Index(string currentCategory, string currentFilter, string searchString, string sortOrder, int? pageNumber)
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
        var users = await _userService.GetAllUsers();

        users = _userService.SortingSelection(users, sortOrder);

        users = _userService.SearchSelection(users, searchString);

        return View("/Pages/Users/Index.cshtml", PaginatedList<UserModel>.Create(users, pageNumber ?? 1, pageSize));
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View("/Pages/Users/Create.cshtml");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RegisterUserModel userModel)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _userService.AddAsync(userModel);
                return User.Identity.IsAuthenticated && User.IsInRole("Administrator") ? RedirectToAction(nameof(Index)) : RedirectToAction(nameof(Login));
            }
        }
        catch (DbUpdateException ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            ModelState.AddModelError("", "Unable to save changes. " + ex);
        }
        return RedirectToAction(nameof(Create));
    }

    [HttpGet]
    [Authorize(Roles = ("Administrator"))]
    [ActionName("Delete")]
    public async Task<IActionResult> Delete(bool? saveChangesError = false)
    {
        Guid? id = TempData["UserId"] as Guid?;
        if (id == null)
        {
            return NotFound();
        }

        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        if (saveChangesError.GetValueOrDefault())
        {
            ViewData["ErrorMessage"] =
                "Delete failed. Try again, and if the problem persists " +
                "see your system administrator.";
        }

        return View("/Pages/Users/Delete.cshtml", user);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = ("Administrator"))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return RedirectToAction(nameof(Delete));
        }

        try
        {

            await _userService.DeleteAsync(id);
        }
        catch (DbUpdateException ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
        }
        return RedirectToAction(nameof(Index));
    }
    public ActionResult Redirect(Guid? id, string name)
    {
        TempData["UserId"] = id;
        return RedirectToAction(name);
    }

    [Authorize(Roles = ("Administrator"))]
    public async Task<IActionResult> Edit()
    {
        Guid? id = TempData["UserId"] as Guid?;

        if (id == null)
        {
            return NotFound();
        }

        var userModel = await _userService.GetByIdAsync(id);

        if (userModel == null)
        {
            return NotFound();
        }

        var editUserModel = _mapper.Map<EditUserModel>(userModel);

        return View("/Pages/Users/Edit.cshtml", editUserModel);
    }

    [HttpPost, ActionName("Edit")]
    [Authorize(Roles = ("Administrator"))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(EditUserModel userModel)
    {
        if (userModel == null)
        {
            return NotFound();
        }

        try
        {
            await _userService.UpdateAsync(userModel);
        }
        catch (DbUpdateException ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            ModelState.AddModelError("", "Unable to save changes. " + ex);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [ActionName("Login")]
    public IActionResult Login()
    {
        return View("/Pages/Users/Login.cshtml");
    }

    [HttpPost, ActionName("Login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginPost(LoginUserModel user)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.LoginAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            //if (result.IsLockedOut)
            //{
            //   // _logger.LogWarning("User account locked out.");
            //    return RedirectToPage("./Lockout");
            //}
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View("/Pages/Users/Login.cshtml");
            }
        }

        return View("/Pages/Users/Login.cshtml");
    }

    [HttpPost, ActionName("Logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogoutPost()
    {
        await _userService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [ActionName("Manage")]
    public IActionResult Manage()
    {
        return View("/Pages/Users/Manage/Index.cshtml");
    }
}
