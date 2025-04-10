using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MikrocopUsers.Api.DTOs.Users;
using MikrocopUsers.IntegrationTests.Infrastructure;

namespace MikrocopUsers.IntegrationTests.Tests;
[Collection(nameof(IntegrationTestCollection))]
public class UsersTests : IntegrationTestFixture
{
    private readonly string _apiKey;

    public UsersTests(WebAppFactory webAppFactory) : base(webAppFactory)
    {
        // Retrieve the API key from the configuration
        using var scope = webAppFactory.Services.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        _apiKey = configuration.GetValue<string>("ApiKey")!;
    }

    private HttpRequestMessage CreateRequest(HttpMethod method, string url, object? content = null)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.Add("X-Api-Key", _apiKey);

        if (content != null)
        {
            request.Content = JsonContent.Create(content);
        }

        return request;
    }

    [Fact]
    public async Task GetUsers_Should_Return_Empty_List_When_No_Users_Exist()
    {
        // Arrange
        await CleanupDatabaseAsync();
        var client = CreateClient();

        // Act
        using var request = CreateRequest(HttpMethod.Get, "/users");
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateUser_Should_Create_User_And_Return_Created_Status()
    {
        // Arrange
        await CleanupDatabaseAsync();
        var client = CreateClient();
        var createUserDto = new CreateUserDto
        {
            UserName = "johndoe",
            FullName = "John Doe",
            Email = "john.doe@example.com",
            MobileNumber = "+123456789",
            Language = "en",
            Culture = "en-US",
            Password = "Password123"
        };

        // Act
        using var request = CreateRequest(HttpMethod.Post, "/users", createUserDto);
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();
        Assert.NotNull(createdUser);
        Assert.Equal(createUserDto.UserName, createdUser.UserName);
        Assert.Equal(createUserDto.FullName, createdUser.FullName);
    }

    [Fact]
    public async Task GetUserById_Should_Return_User_When_User_Exists()
    {
        // Arrange
        await CleanupDatabaseAsync();
        var client = CreateClient();
        var createUserDto = new CreateUserDto
        {
            UserName = "johndoe",
            FullName = "John Doe",
            Email = "john.doe@example.com",
            MobileNumber = "+123456789",
            Language = "en",
            Culture = "en-US",
            Password = "Password123"
        };

        using var createRequest = CreateRequest(HttpMethod.Post, "/users", createUserDto);
        var createResponse = await client.SendAsync(createRequest);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();

        Assert.NotNull(createdUser);

        // Act
        using var request = CreateRequest(HttpMethod.Get, $"/users/{createdUser!.Id}");
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        Assert.NotNull(user);
        Assert.Equal(createdUser.Id, user!.Id);
    }

    [Fact]
    public async Task DeleteUser_Should_Remove_User_When_User_Exists()
    {
        // Arrange
        await CleanupDatabaseAsync();
        var client = CreateClient();
        var createUserDto = new CreateUserDto
        {
            UserName = "johndoe",
            FullName = "John Doe",
            Email = "john.doe@example.com",
            MobileNumber = "+123456789",
            Language = "en",
            Culture = "en-US",
            Password = "Password123"
        };

        using var createRequest = CreateRequest(HttpMethod.Post, "/users", createUserDto);
        var createResponse = await client.SendAsync(createRequest);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();

        Assert.NotNull(createdUser);

        // Act
        using var deleteRequest = CreateRequest(HttpMethod.Delete, $"/users/{createdUser.Id}");
        var deleteResponse = await client.SendAsync(deleteRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        using var getRequest = CreateRequest(HttpMethod.Get, $"/users/{createdUser.Id}");
        var getResponse = await client.SendAsync(getRequest);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task GetUsers_Should_Return_Unauthorized_When_ApiKey_Is_Missing()
    {
        // Arrange
        await CleanupDatabaseAsync();
        var client = CreateClient();

        // Act
        using var request = new HttpRequestMessage(HttpMethod.Get, "/users");
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
