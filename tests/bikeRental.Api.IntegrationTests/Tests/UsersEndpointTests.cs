using System;
using System.Net;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using bikeRental.Api.IntegrationTests.Config;
using bikeRental.Api.IntegrationTests.Config.Constants;
using bikeRental.Api.IntegrationTests.Helpers;
using bikeRental.Application.Models;
using bikeRental.Application.Models.User;
using bikeRental.Core.Identity;
using bikeRental.DataAccess.Persistence;
using NUnit.Framework;

namespace bikeRental.Api.IntegrationTests.Tests;

[TestFixture]
public class UsersEndpointTests : BaseOneTimeSetup
{
    [Test]
    public async Task Create_User_Should_Add_User_To_Database()
    {
        // Arrange
        var context = Host.Services.GetRequiredService<DatabaseContext>();

        var createModel = Builder<CreateUserModel>.CreateNew()
            .With(cu => cu.Email = "IntegrationTest@gmail.com")
            .With(cu => cu.Username = "IntegrationTest")
            .With(cu => cu.Password = "Password.1!")
            .Build();

        // Act
        var apiResponse = await Client.PostAsync("/api/users", new JsonContent(createModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await ResponseHelper.GetApiResultAsync<CreateUserResponseModel>(apiResponse);
        CheckResponse.Succeeded(response);
        context.Users.Should().Contain(u => u.Id.ToString().Equals(response.Result.Id.ToString()));
    }

    [Test]
    public async Task Create_User_Should_Return_BadRequest_If_The_Email_Is_Incorrect()
    {
        // Arrange
        var createModel = Builder<CreateUserModel>.CreateNew()
            .With(cu => cu.Email = "BadEmail")
            .Build();

        // Act
        var apiResponse = await Client.PostAsync("/api/users", new JsonContent(createModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }

    [Test]
    public async Task Create_User_Should_Return_BadRequest_If_The_Username_Is_Incorrect()
    {
        // Arrange
        var createModel = Builder<CreateUserModel>.CreateNew()
            .With(cu => cu.Email = "nuyonu@gmail.com")
            .With(cu => cu.Username = "Len")
            .With(cu => cu.Password = "Password.1!")
            .Build();

        // Act
        var apiResponse = await Client.PostAsync("/api/users", new JsonContent(createModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }

    [Test]
    public async Task Create_User_Should_Return_BadRequest_If_The_Password_Is_Incorrect()
    {
        // Arrange
        var createModel = Builder<CreateUserModel>.CreateNew()
            .With(cu => cu.Email = "nuyonu@gmail.com")
            .With(cu => cu.Password = "incorrect")
            .Build();

        // Act
        var apiResponse = await Client.PostAsync("/api/users", new JsonContent(createModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }


   
    [Test]
    public async Task ConfirmEmail_Should_Return_UnprocessableEntity_If_Token_Or_UserId_Are_Incorrect()
    {
        // Arrange
        var user = Builder<ApplicationUser>.CreateNew()
            .With(u => u.UserName = "ConfirmEmailUser2")
            .With(u => u.Email = "ConfirmEmailUser2@email.com")
            .Build();

        var userManager = Host.Services.GetRequiredService<UserManager<ApplicationUser>>();

        await userManager.CreateAsync(user, "Password.1!");

        var confirmEmailModel = Builder<ConfirmEmailModel>.CreateNew()
            .With(ce => ce.UserId = user.Id.ToString())
            .With(ce => ce.Token = "InvalidToken")
            .Build();

        // Act
        var apiResponse = await Client.PostAsync("/api/users/confirmEmail", new JsonContent(confirmEmailModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }

    [Test]
    public async Task ChangePassword_Should_Return_NotFoundException_If_User_Does_Not_Exists_In_Database()
    {
        // Arrange
        var changePasswordModel = Builder<ChangePasswordModel>.CreateNew()
            .With(cu => cu.OldPassword = "Password.1!")
            .With(cu => cu.NewPassword = "Password.12!")
            .Build();

        // Act
        var apiResponse = await Client.PutAsync($"/api/users/{Guid.NewGuid()}/changePassword",
            new JsonContent(changePasswordModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }

    [Test]
    public async Task ChangePassword_Should_Return_BadRequest_If_OldPassword_Does_Not_Match_User_Password()
    {
        // Arrange
        var user = Builder<ApplicationUser>.CreateNew()
            .With(u => u.UserName = "ChangePasswordBadRequest")
            .With(u => u.Email = "ChangePasswordBadRequest@email.com")
            .Build();

        var userManager = Host.Services.GetRequiredService<UserManager<ApplicationUser>>();

        var createdUser = await userManager.CreateAsync(user, "Password.1!");

        var changePasswordModel = Builder<ChangePasswordModel>.CreateNew()
            .With(cu => cu.OldPassword = "Password.12!")
            .With(cu => cu.NewPassword = "Password.1!")
            .Build();

        // Act
        var apiResponse =
            await Client.PutAsync($"/api/users/{user.Id}/changePassword", new JsonContent(changePasswordModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }

    [Test]
    public async Task ChangePassword_Should_Return_BadRequest_If_NewPassword_Does_Not_Follow_The_Rules()
    {
        // Arrange
        var user = Builder<ApplicationUser>.CreateNew()
            .With(u => u.UserName = "ChangePasswordBadRequest2")
            .With(u => u.Email = "ChangePasswordBadRequest2@email.com")
            .Build();

        var context = (await GetNewHostAsync()).Services.GetRequiredService<DatabaseContext>();

        var userManager = Host.Services.GetRequiredService<UserManager<ApplicationUser>>();

        var createdUser = await userManager.CreateAsync(user, "Password.1!");

        var changePasswordModel = Builder<ChangePasswordModel>.CreateNew()
            .With(cu => cu.OldPassword = "Password.1!")
            .With(cu => cu.NewPassword = "string")
            .Build();

        // Act
        var apiResponse =
            await Client.PutAsync($"/api/users/{user.Id}/changePassword", new JsonContent(changePasswordModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }

    [Test]
    public async Task ChangePassword_Should_Update_User_Password_If_OldPassword_And_NewPassword_Are_Ok()
    {
        // Arrange
        var user = Builder<ApplicationUser>.CreateNew()
            .With(u => u.UserName = "ChangePasswordBadRequest3")
            .With(u => u.Email = "ChangePasswordBadRequest3@email.com")
            .With(u => u.EmailConfirmed = true)
            .Build();

        var userManager = Host.Services.GetRequiredService<UserManager<ApplicationUser>>();

        await userManager.CreateAsync(user, "Password.1!");

        var changePasswordModel = Builder<ChangePasswordModel>.CreateNew()
            .With(cu => cu.OldPassword = "Password.1!")
            .With(cu => cu.NewPassword = "Password.12!")
            .Build();

        // Act
        var apiResponse =
            await Client.PutAsync($"/api/users/{user.Id}/changePassword", new JsonContent(changePasswordModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await ResponseHelper.GetApiResultAsync<BaseResponseModel>(apiResponse);
        CheckResponse.Succeeded(response);
        response.Result.Id.Should().Be(user.Id);
    }
}
