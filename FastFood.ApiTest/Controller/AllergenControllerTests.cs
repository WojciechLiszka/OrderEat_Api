using FastFood.ApiTest.Helpers;
using FastFood.Application.Allergen.Commands.CreateAllergen;
using FastFood.Application.Allergen.Commands.UpdateAllergen;
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
    public class AllergenControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string _route = "/api/allergen";
        private readonly HttpClient _adminClient;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IConfiguration _configuration;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _ownerClient;

        public AllergenControllerTests(WebApplicationFactory<Program> factory)
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
        }

        [Theory]
        [InlineData("", "TestDescription")]
        [InlineData("TestName", "")]
        [InlineData("TestName", null)]
        public async Task Create_ForInvalidModel_ReturnsBadRequest(string name, string description)
        {
            var command = new CreateAllergenCommand()
            {
                Name = name,
                Description = description
            };
            var httpContent = command.ToJsonHttpContent();
            //act

            var response = await _adminClient.PostAsync(_route, httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_ForValidModel_ReturnsCreated()
        {
            //arrange

            var command = new CreateAllergenCommand()
            {
                Name = "Name",
                Description = "TestDecription"
            };
            var httpContent = command.ToJsonHttpContent();
            //act

            var response = await _adminClient.PostAsync(_route, httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task Delete_ForInvalidId_RetunrsNotFound()
        {
            //arrange

            var allergen = new Allergen()
            {
                Name = "TestName",
                Description = "TestDescription"
            };
            await SeedAllergen(allergen);
            //act

            var response = await _adminClient.DeleteAsync($"{_route}/1543");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ForValidId_RetunrsNoContent()
        {
            //arrange

            var allergen = new Allergen()
            {
                Name = "TestName",
                Description = "TestDescription"
            };
            await SeedAllergen(allergen);
            //act

            var response = await _adminClient.DeleteAsync($"{_route}/{allergen.Id}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData("?PageNumber=1&PageSize=7")]
        [InlineData("?SearchPhrase=phrase&PageNumber=1&PageSize=15&SortBy=InvalidProperty")]
        public async Task Get_ForInvalidQueryParams_ReturnsBadRequest(string query)
        {
            //arrange

            var allergen = new Allergen()
            {
                Name = "TestName",
                Description = "TestDescription"
            };
            await SeedAllergen(allergen);
            //act

            var response = await _adminClient.GetAsync($"{_route}{query}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("?PageNumber=1&PageSize=15")]
        [InlineData("?SearchPhrase=phrase&PageNumber=1&PageSize=15")]
        [InlineData("?SearchPhrase=phrase&PageNumber=1&PageSize=15&SortBy=Name")]
        [InlineData("?SearchPhrase=phrase&PageNumber=1&PageSize=15&SortBy=Description")]
        public async Task Get_ForValidQueryParams_ReturnsOk(string query)
        {
            //arrange

            var allergen = new Allergen()
            {
                Name = "TestName",
                Description = "TestDescription"
            };
            await SeedAllergen(allergen);
            //act

            var response = await _adminClient.GetAsync($"{_route}{query}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ForValidId_ReturnsOk()
        {
            //arrange

            var allergen = new Allergen()
            {
                Name = "TestName",
                Description = "TestDescription"
            };
            await SeedAllergen(allergen);
            //act

            var response = await _adminClient.GetAsync($"{_route}/{allergen.Id}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Theory]
        [InlineData("ValidName", null)]
        [InlineData("", "ValidDescription")]
        [InlineData("ValidName", "")]
        public async Task Update_ForInvalidModel_ReturnsBadRequest(string name, string description)
        {
            //arrange

            var allergen = new Allergen()
            {
                Name = "TestName",
                Description = "TestDescription"
            };
            await SeedAllergen(allergen);

            var command = new UpdateAllergenDto()
            {
                Name = name,
                Description = description
            };
            var httpContent = command.ToJsonHttpContent();
            //act

            var response = await _adminClient.PutAsync($"{_route}/{allergen.Id}", httpContent);
            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_ForValidModel_ReturnsOk()
        {
            //arrange

            var allergen = new Allergen()
            {
                Name = "TestName",
                Description = "TestDescription"
            };
            await SeedAllergen(allergen);

            var command = new UpdateAllergenDto()
            {
                Name = "ValidName",
                Description = "ValidDescription"
            };
            var httpContent = command.ToJsonHttpContent();
            //act

            var response = await _adminClient.PutAsync($"{_route}/{allergen.Id}", httpContent);
            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
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

        private async Task SeedAllergen(Allergen allergen)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<FastFoodDbContext>();

            _dbContext.Allergens.Add(allergen);
            await _dbContext.SaveChangesAsync();
        }
    }
}