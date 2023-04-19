using FastFood.ApiTest.Helpers;
using FastFood.Application.SpecialDiet.Commands;
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
    public class SpecialDietControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string _route = "api/specialDiet";
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
                         .AddDbContext<FastFoodDbContext>(options => options.UseInMemoryDatabase("FastFoodDb")
                         .EnableSensitiveDataLogging());
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
        public async Task AddDishToDiet_ForInvalDietIdAndValidDishId_ReturnsNotFound()
        {
            //arrange
            var restaurant = new Restaurant()
            {
                Name = "Test Name",
                Description = "TestDescription",

                ContactDetails = new RestaurantContactDetails
                {
                    ContactNumber = "111111111",
                    Email = "test@email.com",
                    Country = "TestCountry",
                    City = "TestCity",
                    Street = "TestStreet",
                    ApartmentNumber = "1/10"
                }
            };
            await SeedRestaurant(restaurant);

            var diet = new SpecialDiet()
            {
                Name = "Name",
                Description = "Description",
            };
            await seedDiet(diet);

            var dish = new Dish()
            {
                Name = "TestName",
                Description = "description",

                BasePrize = (decimal)10.56,
                BaseCaloricValue = 1000,

                AllowedCustomization = true,
                IsAvilable = true,
                RestaurantId = restaurant.Id
            };
            await seedDish(dish);
            //act

            var response = await _adminClient.PatchAsync($"{_route}/4365/dish/{dish.Id}", null);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddDishToDiet_ForInvalDishIdAndValidDietId_ReturnsNotFound()
        {
            //arrange
            var restaurant = new Restaurant()
            {
                Name = "Test Name",
                Description = "TestDescription",

                ContactDetails = new RestaurantContactDetails
                {
                    ContactNumber = "111111111",
                    Email = "test@email.com",
                    Country = "TestCountry",
                    City = "TestCity",
                    Street = "TestStreet",
                    ApartmentNumber = "1/10"
                }
            };
            await SeedRestaurant(restaurant);

            var diet = new SpecialDiet()
            {
                Name = "Name",
                Description = "Description",
            };
            await seedDiet(diet);

            var dish = new Dish()
            {
                Name = "TestName",
                Description = "description",

                BasePrize = (decimal)10.56,
                BaseCaloricValue = 1000,

                AllowedCustomization = true,
                IsAvilable = true,
                RestaurantId = restaurant.Id
            };
            await seedDish(dish);
            //act

            var response = await _adminClient.PatchAsync($"{_route}/{diet.Id}/dish/6245", null);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddDishToDiet_ForValidId_ReturnsOK()
        {
            //arrange
            var restaurant = new Restaurant()
            {
                Name = "Test Name",
                Description = "TestDescription",

                ContactDetails = new RestaurantContactDetails
                {
                    ContactNumber = "111111111",
                    Email = "test@email.com",
                    Country = "TestCountry",
                    City = "TestCity",
                    Street = "TestStreet",
                    ApartmentNumber = "1/10"
                }
            };
            await SeedRestaurant(restaurant);

            var diet = new SpecialDiet()
            {
                Name = "Name",
                Description = "Description",
            };
            await seedDiet(diet);

            var dish = new Dish()
            {
                Name = "TestName",
                Description = "description",

                BasePrize = (decimal)10.56,
                BaseCaloricValue = 1000,

                AllowedCustomization = true,
                IsAvilable = true,
                RestaurantId = restaurant.Id
            };
            await seedDish(dish);
            //act

            var response = await _adminClient.PatchAsync($"{_route}/{diet.Id}/dish/{dish.Id}", null);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("", "Description")]
        [InlineData("Name", "")]
        [InlineData(null, "Description")]
        [InlineData("name", null)]
        public async Task Create_ForInvalidModel_RetursBadRequest(string name, string description)
        {
            var command = new CreateSpecialDietCommand()
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

            var command = new CreateSpecialDietCommand()
            {
                Name = "TestName",
                Description = "Description"
            };
            var httpContent = command.ToJsonHttpContent();
            //act

            var response = await _adminClient.PostAsync(_route, httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task Delete_ForInvalidId_ReturnsNotFound()
        {
            //arrange
            var diet = new SpecialDiet()
            {
                Name = "Name",
                Description = "Description",
            };
            await seedDiet(diet);

            //act

            var response = await _adminClient.DeleteAsync($"{_route}/3524");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ForValidId_RetursNoContent()
        {
            //arrange
            var diet = new SpecialDiet()
            {
                Name = "Name",
                Description = "Description",
            };
            await seedDiet(diet);

            //act

            var response = await _adminClient.DeleteAsync($"{_route}/{diet.Id}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData("?PageNumber=1&PageSize=7")]
        [InlineData("?SearchPhrase=phrase&PageNumber=1&PageSize=15&SortBy=InvalidProperty")]
        public async Task Get_ForInvalidQueryParams_ReturnsBadRequest(string query)
        {
            //arrange

            var diet = new SpecialDiet()
            {
                Name = "Name",
                Description = "Description"
            };
            await seedDiet(diet);
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

            var diet = new SpecialDiet()
            {
                Name = "Name",
                Description = "Description"
            };
            await seedDiet(diet);
            //act

            var response = await _adminClient.GetAsync($"{_route}{query}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ForInvalidId_ReturnsNotFound()
        {
            //arrange

            var diet = new SpecialDiet()
            {
                Name = "Name",
                Description = "Description"
            };
            await seedDiet(diet);
            //act

            var response = await _adminClient.GetAsync($"{_route}/5436");
            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_ForValidId_ReturnsOk()
        {
            //arrange

            var diet = new SpecialDiet()
            {
                Name = "Name",
                Description = "Description"
            };
            await seedDiet(diet);
            //act

            var response = await _adminClient.GetAsync($"{_route}/{diet.Id}");
            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task Update_ForInvalidId_ReturnsNotFound()
        {
            //arrange

            var diet = new SpecialDiet()
            {
                Name = "Name",
                Description = "Description",
            };
            await seedDiet(diet);

            var dto = new DietDto()
            {
                Name = "TestName",
                Description = "TestDescription",
            };
            var httpContent = dto.ToJsonHttpContent();
            //act

            var response = await _adminClient.PutAsync($"{_route}/35234", httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("", "Description")]
        [InlineData("Name", "")]
        [InlineData(null, "Description")]
        [InlineData("name", null)]
        public async Task Update_ForInvalidModel_ReturnsOK(string name, string description)
        {
            //arrange

            var diet = new SpecialDiet()
            {
                Name = "Name",
                Description = "Description",
            };
            await seedDiet(diet);

            var dto = new DietDto()
            {
                Name = name,
                Description = description,
            };
            var httpContent = dto.ToJsonHttpContent();
            //act

            var response = await _adminClient.PutAsync($"{_route}/{diet.Id}", httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_ForValidModel_RetursOK()
        {
            //arrange

            var diet = new SpecialDiet()
            {
                Name = "Name",
                Description = "Description",
            };
            await seedDiet(diet);

            var dto = new DietDto()
            {
                Name = "TestName",
                Description = "TestDescription",
            };
            var httpContent = dto.ToJsonHttpContent();
            //act

            var response = await _adminClient.PutAsync($"{_route}/{diet.Id}", httpContent);
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

        private async Task seedDiet(SpecialDiet diet)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<FastFoodDbContext>();

            _dbContext.Diets.Add(diet);
            await _dbContext.SaveChangesAsync();
        }

        private async Task seedDish(Dish dish)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<FastFoodDbContext>();

            _dbContext.Dishes.Add(dish);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedRestaurant(Restaurant restaurant)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<FastFoodDbContext>();

            _dbContext.Restaurants.Add(restaurant);
            await _dbContext.SaveChangesAsync();
        }
    }
}