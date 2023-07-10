using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OrderEat.ApiTest.Helpers;
using OrderEat.Domain.Entities;
using OrderEat.Domain.Models;
using OrderEat.Infrastructure.Persistance;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace OrderEat.ApiTest.Controller
{
    public class OrderControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _adminClient;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IConfiguration _configuration;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _ownerClient;
        private readonly HttpClient _userClient;

        public OrderControllerTest(WebApplicationFactory<Program> factory)
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
                            .SingleOrDefault(service => service.ServiceType == typeof(Microsoft.EntityFrameworkCore.DbContextOptions<OrderEatDbContext>));
                        services.Remove(dbContextOptions);

                        services
                         .AddDbContext<OrderEatDbContext>(options => options.UseInMemoryDatabase("FastFoodDb")
                         .EnableSensitiveDataLogging());
                    });
                });

            _adminClient = _factory.CreateClient();
            var adminToken = GenerateJwtToken("Admin", "1");
            _adminClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            _ownerClient = _factory.CreateClient();
            var ownerToken = GenerateJwtToken("Owner", "2");
            _ownerClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ownerToken);
            _userClient = _factory.CreateClient();
            var userToken = GenerateJwtToken("User", "3");
            _userClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        }

        [Fact]
        public async Task AddDishToOrder_ForInvalidDishId_ReturnsNotFound()
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
                }
            };
            var ingredient = new Ingredient()
            {
                Name = "Ingredient",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };

            await SeedRestaurant(restaurant);

            var dish = new Dish()
            {
                Name = "Name",
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
            await Seed(dish);

            var order = new Order()
            {
                Fee = 10,
                UserId = 1,

                Status = OrderStatus.InCart,
                RestaurantId = restaurant.Id,
            };
            await Seed(order);
            var httpContent = new Sheet()
            {
                IngredientsId = new List<int>
                {
                    ingredient.Id
                }
            }
            .ToJsonHttpContent();
            //act

            var response = await _adminClient.PatchAsync($"api/order/{order.Id}/dish/34563", httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddDishToOrder_ForInvalidOrderId_ReturnsNotFound()
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
                }
            };
            var ingredient = new Ingredient()
            {
                Name = "Ingredient",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };

            await SeedRestaurant(restaurant);

            var dish = new Dish()
            {
                Name = "Name",
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
            await Seed(dish);

            var order = new Order()
            {
                Fee = 10,
                UserId = 1,

                Status = OrderStatus.InCart,
                RestaurantId = restaurant.Id,
            };
            await Seed(order);
            var httpContent = new Sheet()
            {
                IngredientsId = new List<int>
                {
                    ingredient.Id
                }
            }
            .ToJsonHttpContent();
            //act

            var response = await _adminClient.PatchAsync($"api/order/6425346/dish/{dish.Id}", httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddDishToOrder_ForInvalidUser_ReturnsForbidden()
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
                }
            };
            var ingredient = new Ingredient()
            {
                Name = "Ingredient",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };

            await SeedRestaurant(restaurant);

            var dish = new Dish()
            {
                Name = "Name",
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
            await Seed(dish);

            var order = new Order()
            {
                Fee = 10,
                UserId = 1,

                Status = OrderStatus.InCart,
                RestaurantId = restaurant.Id,
            };
            await Seed(order);
            var httpContent = new Sheet()
            {
                IngredientsId = new List<int>
                {
                    ingredient.Id
                }
            }
            .ToJsonHttpContent();
            //act

            var response = await _userClient.PatchAsync($"api/order/{order.Id}/dish/{dish.Id}", httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddDishToOrder_ForValidParams_ReturnsOk()
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
                }
            };
            var ingredient = new Ingredient()
            {
                Name = "Ingredient",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };

            await SeedRestaurant(restaurant);

            var dish = new Dish()
            {
                Name = "Name",
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
            await Seed(dish);

            var order = new Order()
            {
                Fee = 10,
                UserId = 1,

                Status = OrderStatus.InCart,
                RestaurantId = restaurant.Id,
                OrderedDishes = new List<OrderedDish>()
            };
            await   Seed(order);
            var httpContent = new Sheet()
            {
                IngredientsId = new List<int>
                {
                    ingredient.Id
                }
            }
            .ToJsonHttpContent();
            //act

            var response = await _adminClient.PatchAsync($"api/order/{order.Id}/dish/{dish.Id}", httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Create_ForInvalidId_ReturnsNotFound()
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
                }
            };
            await SeedRestaurant(restaurant);
            //act

            var response = await _userClient.PostAsync("/api/restaurant/7453/order", null);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_ForValidId_ReturnsCreated()
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
                }
            };
            await SeedRestaurant(restaurant);
            //act

            var response = await _userClient.PostAsync($"/api/restaurant/{restaurant.Id}/order", null);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task GetById_ForInvalidId_ReturnsNotFound()
        {
            //arrange

            var order = new Order()
            {
                Fee = 10,
                UserId = 3,

                Status = OrderStatus.InCart
            };
            await Seed(order);
            //act

            var response = await _userClient.GetAsync("/api/order/54235");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_ForInvalidUser_ReturnsForbidden()
        {
            //arrange

            var order = new Order()
            {
                Fee = 10,
                UserId = 4,

                Status = OrderStatus.InCart
            };
            await Seed(order);
            //act

            var response = await _userClient.GetAsync($"/api/order/{order.Id}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetById_ForValId_ReturnsOk()
        {
            //arrange

            var order = new Order()
            {
                Fee = 10,
                UserId = 3,

                Status = OrderStatus.InCart
            };
            await Seed(order);
            //act

            var response = await _userClient.GetAsync($"/api/order/{order.Id}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task RealizeOrder_ForValidId_ReturnsOK()
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
                }
            };
            var ingredient = new Ingredient()
            {
                Name = "Ingredient",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };

            await SeedRestaurant(restaurant);

            var dish = new Dish()
            {
                Name = "Name",
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
            await Seed(dish);

            var order = new Order()
            {
                Fee = 10,
                UserId = 1,

                Status = OrderStatus.InCart,
                RestaurantId = restaurant.Id,
                OrderedDishes = new List<OrderedDish>()
            };

            await Seed(order);
            var orderedDish = new OrderedDish()
            {
                DishId = dish.Id,
                Name = dish.Name,

                Ingredients = new List<Ingredient>()
                        {
                            ingredient
                        },
                prize = (decimal)10.5
            };
            await AddDishToOrder(order.Id, orderedDish);
            //act

            var response = await _adminClient.PatchAsync($"/api/order/{order.Id}", null);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task RealizeOrder_ForInvalidId_ReturnsNotFound()
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
                }
            };
            var ingredient = new Ingredient()
            {
                Name = "Ingredient",
                Description = "Description",

                Prize = (decimal)10.5,
                IsRequired = true
            };

            await SeedRestaurant(restaurant);

            var dish = new Dish()
            {
                Name = "Name",
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
            await Seed(dish);

            var order = new Order()
            {
                Fee = 10,
                UserId = 1,

                Status = OrderStatus.InCart,
                RestaurantId = restaurant.Id,
                OrderedDishes = new List<OrderedDish>()
            };

            await Seed(order);
            var orderedDish = new OrderedDish()
            {
                DishId = dish.Id,
                Name = dish.Name,

                Ingredients = new List<Ingredient>()
                        {
                            ingredient
                        },
                prize = (decimal)10.5
            };
            await AddDishToOrder(order.Id, orderedDish);
            //act

            var response = await _adminClient.PatchAsync("/api/order/6453", null);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        private async Task AddDishToOrder(int id, OrderedDish orderedDish)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<OrderEatDbContext>();

            var order = await _dbContext.Orders
                .Include(x => x.OrderedDishes)
                .FirstOrDefaultAsync(x => x.Id == id);

            order.OrderedDishes = new List<OrderedDish>()
            {
                orderedDish
            };
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

        private async Task Seed<T>(T obj) where T : class
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<OrderEatDbContext>();

            _dbContext.Add(obj);

            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedRestaurant(Restaurant restaurant)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<OrderEatDbContext>();

            _dbContext.Add(restaurant);
            await _dbContext.SaveChangesAsync();
        }
    }
}