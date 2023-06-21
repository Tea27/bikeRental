using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using bikeRental.Application.Common.Email;
using bikeRental.Application.Exceptions;
using bikeRental.Application.Helpers;
using bikeRental.Application.Models;
using bikeRental.Application.Models.User;
using bikeRental.Application.Templates;
using bikeRental.Core.Identity;
using bikeRental.DataAccess.Repositories;
using bikeRental.DataAccess.Repositories.Impl;
using bikeRental.Application.Models.Station;
using bikeRental.Core.Enums;
using System.Data.Entity;
using bikeRental.Core.Entities;
using Microsoft.AspNetCore.Routing.Template;

namespace bikeRental.Application.Services.Impl;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITemplateService _templateService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository<ApplicationUser> _userRepository;

    public UserService(IMapper mapper,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        ITemplateService templateService,
        IEmailService emailService,
        IUserRepository<ApplicationUser> userRepository)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _templateService = templateService;
        _emailService = emailService;
        _userRepository = userRepository;
    }

    public async Task<CreateUserResponseModel> CreateAsync(CreateUserModel createUserModel)
    {
        var user = _mapper.Map<ApplicationUser>(createUserModel);

        var result = await _userManager.CreateAsync(user, createUserModel.Password);

        if (!result.Succeeded) throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var emailTemplate = await _templateService.GetTemplateAsync(TemplateConstants.ConfirmationEmail);

        var emailBody = _templateService.ReplaceInTemplate(emailTemplate,
            new Dictionary<string, string> { { "{UserId}", user.Id.ToString() }, { "{Token}", token } });

        await _emailService.SendEmailAsync(EmailMessage.Create(user.Email, emailBody, "[bikeRental]Confirm your email"));

        return new CreateUserResponseModel
        {
            Id = Guid.Parse((await _userManager.FindByNameAsync(user.UserName)).Id.ToString())
        };
    }


    public async Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel)
    {
        var user = await _userManager.FindByIdAsync(confirmEmailModel.UserId);

        if (user == null)
            throw new UnprocessableRequestException("Your verification link is incorrect");

        var result = await _userManager.ConfirmEmailAsync(user, confirmEmailModel.Token);

        if (!result.Succeeded)
            throw new UnprocessableRequestException("Your verification link has expired");

        return new ConfirmEmailResponseModel
        {
            Confirmed = true
        };
    }



    public async Task<IEnumerable<UserModel>> GetAllUsers()
    {
        var response = await _userRepository.GetAllAsync();
        var users = new List<UserModel>();
        foreach (var user in response)
        {
            var userDto = _mapper.Map<UserModel>(user);
            try
            {
                var role = _userManager.GetRolesAsync(user).Result.First();
                userDto.Role = (Role)Enum.Parse(typeof(Role), role);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            users.Add(userDto);
        }
        return users;
    }
    public async Task AddAsync(RegisterUserModel userModel)
    {
        var user = _mapper.Map<ApplicationUser>(userModel);
        await _userRepository.AddAsync(user, userModel.Role.ToString(), userModel.Password);
    }

    public async Task<UserModel> GetByIdAsync(Guid? id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        var userModel = _mapper.Map<UserModel>(user);

        var role = _userManager.GetRolesAsync(user).Result.First();
        userModel.Role = (Role)Enum.Parse(typeof(Role), role);

        return userModel;
    }

    public async Task<SignInResult> LoginAsync(LoginUserModel userModel)
    {
        return await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, userModel.RememberMe, lockoutOnFailure: false);
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
    public async Task DeleteAsync(Guid Id)
    {
        await _userRepository.DeleteAsync(Id);
    }

    public async Task UpdateAsync(EditUserModel userModel)
    {

        var newRole = userModel.Role.ToString();

        var user = _mapper.Map<ApplicationUser>(userModel);

        await _userRepository.UpdateAsync(user, newRole);
    }

    public IEnumerable<UserModel> SearchSelection(IEnumerable<UserModel> users, string searchString)
    {
        IEnumerable<UserModel> usersSearched = users.ToList();

        if (!String.IsNullOrEmpty(searchString))
        {
            var searchStrTrim = searchString.Trim();
            usersSearched = users.Where(t => t.FirstName.Contains(searchStrTrim)
                                          || t.LastName.Contains(searchStrTrim)
                                          || t.UserName.Contains(searchStrTrim)
                                          || t.Email.Contains(searchStrTrim)
                                          );
        }
        return usersSearched;
    }
    public IEnumerable<UserModel> SortingSelection(IEnumerable<UserModel> users, string sortOrder)
    {
        switch (sortOrder)
        {
            case "FirstName":
                return users.OrderBy(s => s.FirstName);
            case "FirstNameDesc":
                return users = users.OrderByDescending(s => s.FirstName);
            case "LastName":
                return users.OrderBy(s => s.LastName);
            case "LastNameDesc":
                return users.OrderByDescending(s => s.LastName);
            case "UserName":
                return users.OrderBy(s => s.UserName);
            case "UserNameDesc":
                return users.OrderByDescending(s => s.UserName);
            case "Email":
                return users.OrderBy(s => s.Email);
            case "EmailDesc":
                return users.OrderByDescending(s => s.Email);
            default:
                return users.OrderBy(s => s.LastName);
        }
    }
}
