using bikeRental.Application;
using bikeRental.Application.Models.User;
using bikeRental.Application.Services;
using bikeRental.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using bikeRental.Core.Enums;

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

        var users  = _userService.CheckSwitch(filterString, searchString, sortOrder);

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
                return User.IsInRole("Administrator") ? RedirectToAction(nameof(Index)) : RedirectToAction(nameof(Login));
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
            return BadRequest();
        }

        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return BadRequest();
        }

        if (saveChangesError.GetValueOrDefault())
        {
            ViewData["ErrorMessage"] =
                 "User cannot be removed, it has order relation";
        }

        return View("/Pages/Users/Delete.cshtml", user);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = ("Administrator"))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var user = await _userService.GetByIdAsyncIncludeOrders(id);
        TempData["UserId"] = id;
        if (user == null)
        {
            return RedirectToAction(nameof(Delete));
        }

        try
        {
            if (user.Orders.Any())
            {
                throw new DbUpdateException();
            }
            await _userService.DeleteAsync(id);
        }
        catch (DbUpdateException ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            return RedirectToAction(nameof(Delete), new {  saveChangesError = true });
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
            return BadRequest();
        }

        var userModel = await _userService.GetByIdAsync(id);

        if (userModel == null)
        {
            return BadRequest();
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
        var userLoging = await _userManager.FindByEmailAsync(user.Email);
        if (userLoging.Status == AccountStatus.Disabled) return BadRequest("Your account is disabled");

        if (ModelState.IsValid)
        {
            var result = await _userService.LoginAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
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
    public async Task<IActionResult> Disable(Guid id)
    {
        var customer = await _userService.GetByIdAsync(id);
        customer.Status = AccountStatus.Disabled;
        var editUserModel = _mapper.Map<EditUserModel>(customer);

        await _userService.UpdateAsync(editUserModel);

        return RedirectToAction(nameof(Index));
    }

}
