﻿using FastFood.ApiTest.Helpers;
using FastFood.Application.Account.Command.RegisterUser;
using FastFood.Infrastructure.Persistance;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FastFood.ApiTest.Controller
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;
        private const string _route = "api/account";

        public AccountControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(Microsoft.EntityFrameworkCore.DbContextOptions<FastFoodDbContext>));
                        services.Remove(dbContextOptions);

                        services
                         .AddDbContext<FastFoodDbContext>(options => options.UseInMemoryDatabase("FastFoodDb"));
                    });
                });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task RegisterUser_ForValidRegisterUserDto_ReturnsOk()
        {
            // arrange

            var command = new RegisterUserCommand()
            {
                Email = "test@test.com",
                Password = "testpassword",
                ConfirmPassword = "testpassword",
                Name = "John Doe"
            };
            var httpContent = command.ToJsonHttpContent();
            // act

            var response = await _client.PostAsync($"{_route}/register", httpContent);
            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("test@test.com", "testpassword", "notthesamepassword", "John Doe")]
        [InlineData("test@test.com", null, null, "John Doe")]
        [InlineData("test@test.com", "testpassword", "testpassword", null)]
        [InlineData(null, "testpassword", "testpassword", "John Doe")]
        [InlineData("", "testpassword", "testpassword", "John Doe")]
        [InlineData("test@test.com", "testpassword", "testpassword", "")]
        [InlineData("test@test.com", "", "", "John Doe")]
        public async Task RegisterUser_ForInvalidRegisterUserDto_BadRequest(string email, string password, string confirmPassword, string name)
        {
            // arrange

            var command = new RegisterUserCommand()
            {
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword,
                Name = name
            };
            var httpContent = command.ToJsonHttpContent();
            // act

            var response = await _client.PostAsync($"{_route}/register", httpContent);
            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}