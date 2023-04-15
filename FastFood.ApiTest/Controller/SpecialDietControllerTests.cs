using FastFood.ApiTest.Helpers;
using FastFood.Application.SpecialDiet.Commands.CreateSpecialDiet;
using FastFood.Domain.Entities;
using FastFood.Domain.Models;
using FastFood.Infrastructure.Persistance;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace FastFood.ApiTest.Controller
{
    public class SpecialDietControllerTests :IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _adminClient;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IConfiguration _configuration;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _ownerClient;

        public SpecialDietControllerTests(WebApplicationFactory<Program> factory)
        {
            _configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();
            var authenticationSettings = new AuthenticationSettings();

            _configuration.GetSection("Authentication").Bind(authenticationSettings);
            _authenticationSettings = authenticationSettings;
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(Microsoft.EntityFrameworkCore.DbContextOptions<FastFoodDbContext>));
                        services.Remove(dbContextOptions);

                        services
                         .AddDbContext<FastFoodDbContext>(options => options.UseInMemoryDatabase("FastFoodDb"));
                    });
                });

            _adminClient = _factory.CreateClient();
            var adminToken = GenerateJwtToken("Admin", "1");
            _adminClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            _ownerClient = _factory.CreateClient();
            var ownerToken = GenerateJwtToken("Owner", "2");
            _ownerClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ownerToken);
        }

        [Fact]
        public async Task Create_ForValidModel_ReturnsCreated()
        {
            //arrange

            var command = new CreateSpecialDietCommand()
            {
                Name = "Name",
                Description = "Description"
            };
            var httpContent = command.ToJsonHttpContent();
            //act

            var response = await _adminClient.PostAsync("api/specialDiet", httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        private async Task seedDiet(SpecialDiet diet)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<FastFoodDbContext>();

            _dbContext.Diets.Add(diet);
            await _dbContext.SaveChangesAsync();
        }

        private string GenerateJwtToken(string roleName, string userId)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email,"test@email.com"),
                new Claim(ClaimTypes.Name, "John Doe"),
                new Claim(ClaimTypes.Role, roleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}