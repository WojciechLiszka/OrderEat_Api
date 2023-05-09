using FastFood.ApiTest.Helpers;
using FastFood.Application.Ingredient.Command;
using FastFood.Application.Ingredient.Command.UpdateIngredient;
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
    public class IngredientControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string _route = "/api";
        private readonly HttpClient _adminClient;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IConfiguration _configuration;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _ownerClient;

        public IngredientControllerTests(WebApplicationFactory<Program> factory)
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

        [Theory]
        [InlineData(null, "Description", 10.5, true)]
        [InlineData("TestName", "", 10.5, true)]
        [InlineData("", "TestDescription", 10.5, true)]
        [InlineData("TestName", "TestDescription", 0, true)]
        public async Task Create_ForInvalidModelAndValidId_ReturnsBadRequest(string name, string description, decimal prize, bool isRequired)
        {
            //arrange
            var restaurant = new Restaurant()
            {
                Name = "Name",
                Description = "TestDescription",
                ContactDetails = new RestaurantContactDetails
                {
                    ContactNumber = "111111111",
                    Email = "test@email.com",
                    Country = "TestCountry",
                    City = "TestCity",
                    Street = "TestStreet",
                    ApartmentNumber = "1/10"
                },
                CreatedById = 3
            };
            await SeedRestaurant(restaurant);

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
            await SeedDish(dish);

            var dto = new IngredientDto()
            {
                Name = name,
                Description = description,
                Prize = prize,
                IsRequired = isRequired
            };
            var httpContent = dto.ToJsonHttpContent();
            //act

            var response = await _adminClient.PostAsync($"{_route}/dish/{dish.Id}/ingredient", httpContent);
            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_ForNotRestauranOwner_ReturnsForbidden()
        {
            //arrange
            var restaurant = new Restaurant()
            {
                Name = "RestaurantName",
                Description = "TestDescription",
                ContactDetails = new RestaurantContactDetails
                {
                    ContactNumber = "111111111",
                    Email = "test@email.com",
                    Country = "TestCountry",
                    City = "TestCity",
                    Street = "TestStreet",
                    ApartmentNumber = "1/10"
                },
                CreatedById = 3
            };
            await SeedRestaurant(restaurant);

            var dish = new Dish()
            {
                Name = "DishName",
                Description = "description",

                BasePrize = (decimal)10.56,
                BaseCaloricValue = 1000,

                AllowedCustomization = true,
                IsAvilable = true,
                RestaurantId = restaurant.Id
            };
            await SeedDish(dish);

            var dto = new IngredientDto()
            {
                Name = "Name",
                Description = "Description",
                Prize = (decimal)10.50,
                IsRequired = true
            };
            var httpContent = dto.ToJsonHttpContent();
            //act

            var response = await _ownerClient.PostAsync($"{_route}/dish/{dish.Id}/ingredient", httpContent);
            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_ForValidModelAndDishId_ReturnsCreated()
        {
            //arrange
            var restaurant = new Restaurant()
            {
                Name = "Name",
                Description = "TestDescription",
                ContactDetails = new RestaurantContactDetails
                {
                    ContactNumber = "111111111",
                    Email = "test@email.com",
                    Country = "TestCountry",
                    City = "TestCity",
                    Street = "TestStreet",
                    ApartmentNumber = "1/10"
                },
                CreatedById = 2
            };
            await SeedRestaurant(restaurant);

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
            await SeedDish(dish);

            var dto = new IngredientDto()
            {
                Name = "Name",
                Description = "Description",
                Prize = (decimal)10.50,
                IsRequired = true
            };
            var httpContent = dto.ToJsonHttpContent();
            //act

            var response = await _adminClient.PostAsync($"{_route}/dish/{dish.Id}/ingredient", httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task Create_ForValidModelAndInvalidDishId_ReturnsNotFound()
        {
            //arrange
            var restaurant = new Restaurant()
            {
                Name = "Name",
                Description = "TestDescription",
                ContactDetails = new RestaurantContactDetails
                {
                    ContactNumber = "111111111",
                    Email = "test@email.com",
                    Country = "TestCountry",
                    City = "TestCity",
                    Street = "TestStreet",
                    ApartmentNumber = "1/10"
                },
                CreatedById = 2
            };
            await SeedRestaurant(restaurant);

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
            await SeedDish(dish);

            var dto = new IngredientDto()
            {
                Name = "Name",
                Description = "Description",
                Prize = (decimal)10.50,
                IsRequired = true
            };
            var httpContent = dto.ToJsonHttpContent();
            //act

            var response = await _adminClient.PostAsync($"{_route}/dish/63456/ingredient", httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ForInvalidIdReturns_NotFound()
        {
            //arrange

            var ingredient = new Ingredient()
            {
                Name = "Ingredient",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };
            await SeedIngredient(ingredient);

            //act

            var response = await _adminClient.DeleteAsync($"{_route}/ingredient/7656");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ForNotRestautantOwner_ReturnsForbiden()
        {
            //arrange

            var restaurant = new Restaurant()
            {
                Name = "Name",
                Description = "TestDescription",
                ContactDetails = new RestaurantContactDetails
                {
                    ContactNumber = "111111111",
                    Email = "test@email.com",
                    Country = "TestCountry",
                    City = "TestCity",
                    Street = "TestStreet",
                    ApartmentNumber = "1/10"
                },
                CreatedById = 4
            };
            await SeedRestaurant(restaurant);

            var ingredient = new Ingredient()
            {
                Name = "Ingredient",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };

            var dish = new Dish()
            {
                Name = "TestName",
                Description = "description",

                BasePrize = (decimal)10.56,
                BaseCaloricValue = 1000,

                AllowedCustomization = true,
                IsAvilable = true,
                RestaurantId = restaurant.Id,
                AllowedIngreedients = new List<Ingredient>
                {
                    ingredient
                }
            };
            await SeedDish(dish);

            //act

            var response = await _ownerClient.DeleteAsync($"{_route}/ingredient/{ingredient.Id}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_ForValidIdReturns_NoContent()
        {
            //arrange

            var restaurant = new Restaurant()
            {
                Name = "Name",
                Description = "TestDescription",
                ContactDetails = new RestaurantContactDetails
                {
                    ContactNumber = "111111111",
                    Email = "test@email.com",
                    Country = "TestCountry",
                    City = "TestCity",
                    Street = "TestStreet",
                    ApartmentNumber = "1/10"
                },
                CreatedById = 4
            };
            await SeedRestaurant(restaurant);

            var ingredient = new Ingredient()
            {
                Name = "Ingredient",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };

            var dish = new Dish()
            {
                Name = "TestName",
                Description = "description",

                BasePrize = (decimal)10.56,
                BaseCaloricValue = 1000,

                AllowedCustomization = true,
                IsAvilable = true,
                RestaurantId = restaurant.Id,
                AllowedIngreedients = new List<Ingredient>
                {
                    ingredient
                }
            };
            await SeedDish(dish);

            //act

            var response = await _adminClient.DeleteAsync($"{_route}/ingredient/{ingredient.Id}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GetById_ForInvalidId_ReturnsNotFound()
        {
            //arrange

            var restaurant = new Restaurant()
            {
                Name = "Name",
                Description = "TestDescription",
                ContactDetails = new RestaurantContactDetails
                {
                    ContactNumber = "111111111",
                    Email = "test@email.com",
                    Country = "TestCountry",
                    City = "TestCity",
                    Street = "TestStreet",
                    ApartmentNumber = "1/10"
                },
                CreatedById = 4
            };
            await SeedRestaurant(restaurant);

            var ingredient = new Ingredient()
            {
                Name = "Ingredient",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };

            var dish = new Dish()
            {
                Name = "TestName",
                Description = "description",

                BasePrize = (decimal)10.56,
                BaseCaloricValue = 1000,

                AllowedCustomization = true,
                IsAvilable = true,
                RestaurantId = restaurant.Id,
                AllowedIngreedients = new List<Ingredient>
                {
                    ingredient
                }
            };
            await SeedDish(dish);
            //act

            var response = await _adminClient.GetAsync($"{_route}/ingredient/23564");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_ForValidId_ReturnsOk()
        {
            //arrange

            var restaurant = new Restaurant()
            {
                Name = "Name",
                Description = "TestDescription",
                ContactDetails = new RestaurantContactDetails
                {
                    ContactNumber = "111111111",
                    Email = "test@email.com",
                    Country = "TestCountry",
                    City = "TestCity",
                    Street = "TestStreet",
                    ApartmentNumber = "1/10"
                },
                CreatedById = 4
            };
            await SeedRestaurant(restaurant);

            var ingredient = new Ingredient()
            {
                Name = "Ingredient",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };

            var dish = new Dish()
            {
                Name = "TestName",
                Description = "description",

                BasePrize = (decimal)10.56,
                BaseCaloricValue = 1000,

                AllowedCustomization = true,
                IsAvilable = true,
                RestaurantId = restaurant.Id,
                AllowedIngreedients = new List<Ingredient>
                {
                    ingredient
                }
            };
            await SeedDish(dish);
            //act

            var response = await _adminClient.GetAsync($"{_route}/ingredient/{ingredient.Id}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetFromDish_ForValidId_ReturnsOk()
        {
            //arrange

            var restaurant = new Restaurant()
            {
                Name = "Name",
                Description = "TestDescription",
                ContactDetails = new RestaurantContactDetails
                {
                    ContactNumber = "111111111",
                    Email = "test@email.com",
                    Country = "TestCountry",
                    City = "TestCity",
                    Street = "TestStreet",
                    ApartmentNumber = "1/10"
                },
                CreatedById = 4
            };
            await SeedRestaurant(restaurant);

            var ingredient = new Ingredient()
            {
                Name = "Ingredient",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };

            var dish = new Dish()
            {
                Name = "TestName",
                Description = "description",

                BasePrize = (decimal)10.56,
                BaseCaloricValue = 1000,

                AllowedCustomization = true,
                IsAvilable = true,
                RestaurantId = restaurant.Id,
                AllowedIngreedients = new List<Ingredient>
                {
                    ingredient
                }
            };
            await SeedDish(dish);
            //act

            var response = await _adminClient.GetAsync($"{_route}/dish/{dish.Id}/ingredient");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetFromDish_FormInvalidId_ReturnsNotFound()
        {
            //arrange

            var restaurant = new Restaurant()
            {
                Name = "Name",
                Description = "TestDescription",
                ContactDetails = new RestaurantContactDetails
                {
                    ContactNumber = "111111111",
                    Email = "test@email.com",
                    Country = "TestCountry",
                    City = "TestCity",
                    Street = "TestStreet",
                    ApartmentNumber = "1/10"
                },
                CreatedById = 4
            };
            await SeedRestaurant(restaurant);

            var ingredient = new Ingredient()
            {
                Name = "Ingredient",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };

            var dish = new Dish()
            {
                Name = "TestName",
                Description = "description",

                BasePrize = (decimal)10.56,
                BaseCaloricValue = 1000,

                AllowedCustomization = true,
                IsAvilable = true,
                RestaurantId = restaurant.Id,
                AllowedIngreedients = new List<Ingredient>
                {
                    ingredient
                }
            };
            await SeedDish(dish);
            //act

            var response = await _adminClient.GetAsync($"{_route}/dish/5636/ingredient");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(null, "Description", 10.5, true)]
        [InlineData("TestName", "", 10.5, true)]
        [InlineData("", "TestDescription", 10.5, true)]
        [InlineData("TestName", "TestDescription", 0, true)]
        [InlineData("TestName", "TestDescription", 10.5, null)]
        public async Task Update_ForInvalidModelAndValidId_ReturnsBadRequest(string name, string description, decimal prize, bool isRequired)
        {
            //arrange

            var ingredient = new Ingredient()
            {
                Name = "Name",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };
            await SeedIngredient(ingredient);

            var dto = new UpdateIngredientCommand()
            {
                Id = ingredient.Id,
                Name = name,
                Description = description,
                Prize = prize,
                IsRequired = isRequired
            };
            var httpContent = dto.ToJsonHttpContent();
            //act

            var response = await _adminClient.PutAsync($"{_route}/ingredient", httpContent);
            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_ForNotRestaurantOwner_ReturnsForbidden()
        {
            var restaurant = new Restaurant()
            {
                Name = "Name",
                Description = "TestDescription",
                ContactDetails = new RestaurantContactDetails
                {
                    ContactNumber = "111111111",
                    Email = "test@email.com",
                    Country = "TestCountry",
                    City = "TestCity",
                    Street = "TestStreet",
                    ApartmentNumber = "1/10"
                },
                CreatedById = 4
            };
            await SeedRestaurant(restaurant);

            var ingredient = new Ingredient()
            {
                Name = "Ingredient",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };

            var dish = new Dish()
            {
                Name = "TestName",
                Description = "description",

                BasePrize = (decimal)10.56,
                BaseCaloricValue = 1000,

                AllowedCustomization = true,
                IsAvilable = true,
                RestaurantId = restaurant.Id,
                AllowedIngreedients = new List<Ingredient>
                {
                    ingredient
                }
            };
            await SeedDish(dish);

            var dto = new UpdateIngredientCommand()
            {
                Id = ingredient.Id,
                Name = "Name",
                Description = "Description",
                Prize = (decimal)10.50,
                IsRequired = true,
            };
            var httpContent = dto.ToJsonHttpContent();
            //act

            var response = await _ownerClient.PutAsync($"{_route}/ingredient", httpContent);
            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_ForValidModelAndId_ReturnsOk()
        {
            //arrange

            var restaurant = new Restaurant()
            {
                Name = "Name",
                Description = "TestDescription",
                ContactDetails = new RestaurantContactDetails
                {
                    ContactNumber = "111111111",
                    Email = "test@email.com",
                    Country = "TestCountry",
                    City = "TestCity",
                    Street = "TestStreet",
                    ApartmentNumber = "1/10"
                },
                CreatedById = 4
            };
            await SeedRestaurant(restaurant);

            var ingredient = new Ingredient()
            {
                Name = "Ingredient",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };

            var dish = new Dish()
            {
                Name = "TestName",
                Description = "description",

                BasePrize = (decimal)10.56,
                BaseCaloricValue = 1000,

                AllowedCustomization = true,
                IsAvilable = true,
                RestaurantId = restaurant.Id,
                AllowedIngreedients = new List<Ingredient>
                {
                    ingredient
                }
            };
            await SeedDish(dish);

            var dto = new UpdateIngredientCommand()
            {
                Id = ingredient.Id,
                Name = "Name",
                Description = "Description",
                Prize = (decimal)10.50,
                IsRequired = true
            };
            var httpContent = dto.ToJsonHttpContent();
            //act

            var response = await _adminClient.PutAsync($"{_route}/ingredient", httpContent);
            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Update_ForValidModelAndInvalidId_ReturnsNotFound()
        {
            //arrange

            var ingredient = new Ingredient()
            {
                Name = "Name",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };
            await SeedIngredient(ingredient);

            var dto = new UpdateIngredientCommand()
            {
                Id = 4532,
                Name = "Name",
                Description = "Description",
                Prize = (decimal)10.50,
                IsRequired = true
            };
            var httpContent = dto.ToJsonHttpContent();
            //act

            var response = await _adminClient.PutAsync($"{_route}/ingredient", httpContent);
            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
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

        private async Task SeedDish(Dish dish)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<FastFoodDbContext>();

            _dbContext.Add(dish);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedIngredient(Ingredient ingredient)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<FastFoodDbContext>();

            _dbContext.Add(ingredient);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedRestaurant(Restaurant restaurant)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<FastFoodDbContext>();

            _dbContext.Add(restaurant);
            await _dbContext.SaveChangesAsync();
        }
    }
}