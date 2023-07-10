using OrderEat.ApiTest.Helpers;
using OrderEat.Application.Restaurant.Commands.CreateRestaurant;
using OrderEat.Application.Restaurant.Commands.UpdateRestaurant;
using OrderEat.Domain.Entities;
using OrderEat.Domain.Models;
using OrderEat.Infrastructure.Persistance;
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

namespace OrderEat.ApiTest.Controller
{
    public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string _route = "/api/restaurant";
        private readonly HttpClient _adminClient;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IConfiguration _configuration;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _ownerClient;

        public RestaurantControllerTests(WebApplicationFactory<Program> factory)
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
        }

        [Theory]
        [InlineData("TestName", "TestDescription", "111111111", "test@email.com", "TestCountry", "TestCity", "TestStreet", "")]
        [InlineData("TestName", "TestDescription", "111111111", "test@email.com", "TestCountry", "TestCity", "", "1/10")]
        [InlineData("TestName", "TestDescription", "111111111", "test@email.com", "TestCountry", "", "TestStreet", "1/10")]
        [InlineData("TestName", "TestDescription", "111111111", "test@email.com", "", "TestCity", "TestStreet", "1/10")]
        [InlineData("", "TestDescription", "111111111", "test@email.com", "TestCountry", "TestCity", "TestStreet", "1/10")]
        [InlineData("TestName", "", "111111111", "test@email.com", "TestCountry", "TestCity", "TestStreet", "1/10")]
        [InlineData("TestName", "TestDescription", "111111111", "", "TestCountry", "TestCity", "TestStreet", "1/10")]
        [InlineData("TestName", "TestDescription", "111111111", "test@email.com", "TestCountry", "TestCity", "TestStreet", null)]
        [InlineData("TestName", "TestDescription", "111111111", "test@email.com", "TestCountry", "TestCity", null, "1/10")]
        [InlineData("TestName", "TestDescription", "111111111", "test@email.com", "TestCountry", null, "TestStreet", "1/10")]
        [InlineData("TestName", "TestDescription", "111111111", "test@email.com", null, "TestCity", "TestStreet", "1/10")]
        [InlineData("TestName", null, "111111111", "test@email.com", "TestCountry", "TestCity", "TestStreet", "1/10")]
        [InlineData("TestName", "TestDescription", "111111111", null, "TestCountry", "TestCity", "TestStreet", "1/10")]
        public async Task CreateRestaurant_ForInvalidModel_BadRequest(string name, string description, string contactNumber, string email, string country, string city, string street, string apartmentNumber)
        {
            //arrange

            var command = new CreateRestaurantCommand()
            {
                Name = name,
                Description = description,
                ContactNumber = contactNumber,
                Email = email,
                Country = country,
                City = city,
                Street = street,
                ApartmentNumber = apartmentNumber
            };

            var httpContent = command.ToJsonHttpContent();
            //act

            var response = await _adminClient.PostAsync(_route, httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateRestaurant_ForValidModel_ReturnsCreated()
        {
            //arrange

            var command = new CreateRestaurantCommand()
            {
                Name = "UniqueName",
                Description = "TestDescription",
                ContactNumber = "111111111",
                Email = "test@email.com",
                Country = "TestCountry",
                City = "TestCity",
                Street = "TestStreet",
                ApartmentNumber = "1/10"
            };

            var httpContent = command.ToJsonHttpContent();
            //act

            var response = await _adminClient.PostAsync(_route, httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task Delete_ForInvalidId_RetursNotFound()
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

            await Seed(restaurant);
            //act

            var response = await _adminClient.DeleteAsync($"{_route}/63345");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ForNotRestaurantOwner_ReturnForbidden()
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
                CreatedById = 21451
            };
            await Seed(restaurant);
            //act

            var response = await _ownerClient.DeleteAsync($"{_route}/{restaurant.Id}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_ForRestaurantOwner_ReturnsNoContent()
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
            await Seed(restaurant);
            //act

            var response = await _ownerClient.DeleteAsync($"{_route}/{restaurant.Id}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ForValidId_RetursNoContent()
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

            await Seed(restaurant);
            //act

            var response = await _adminClient.DeleteAsync($"{_route}/{restaurant.Id}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData("?PageNumber=1&PageSize=7")]
        [InlineData("?SearchPhrase=phrase&PageNumber=1&PageSize=15&SortBy=InvalidProperty")]
        public async Task Get_ForInValidQuery_BadRequest(string query)
        {
            //arrange

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
        public async Task Get_ForValidQuery_ReturnOk(string query)
        {
            //arrange

            //act

            var response = await _adminClient.GetAsync($"{_route}{query}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ForInvalId_ReturnNotFound()
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
            await Seed(restaurant);
            //act

            var response = await _adminClient.GetAsync($"{_route}/452345");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_ForValidId_ReturnOk()
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
            await Seed(restaurant);
            //act

            var response = await _adminClient.GetAsync($"{_route}/{restaurant.Id}");
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Update_ForInvalidId_ReturnsNotFound()
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
            await Seed(restaurant);

            var command = new UpdateRestaurantCommand()
            {
                Description = "ValidTestDescription",
                ContactNumber = "111111111",
                Email = "Validtest@email.com",
                Country = "ValidTestCountry",
                City = "ValidTestCity",
                Street = "ValidTestStreet",
                ApartmentNumber = "1/10"
            };
            var httpContent = command.ToJsonHttpContent();
            //act

            var response = await _adminClient.PutAsync($"{_route}/34524", httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("TestDescription", "111111111", "test@email.com", "TestCountry", "TestCity", "TestStreet", "")]
        [InlineData("TestDescription", "111111111", "test@email.com", "TestCountry", "TestCity", "", "1/10")]
        [InlineData("TestDescription", "111111111", "test@email.com", "TestCountry", "", "TestStreet", "1/10")]
        [InlineData("TestDescription", "111111111", "test@email.com", "", "TestCity", "TestStreet", "1/10")]
        [InlineData("", "111111111", "test@email.com", "TestCountry", "TestCity", "TestStreet", "1/10")]
        [InlineData("TestDescription", "111111111", "", "TestCountry", "TestCity", "TestStreet", "1/10")]
        [InlineData("TestDescription", "111111111", "test@email.com", "TestCountry", "TestCity", "TestStreet", null)]
        [InlineData("TestDescription", "111111111", "test@email.com", "TestCountry", "TestCity", null, "1/10")]
        [InlineData("TestDescription", "111111111", "test@email.com", "TestCountry", null, "TestStreet", "1/10")]
        [InlineData("TestDescription", "111111111", "test@email.com", null, "TestCity", "TestStreet", "1/10")]
        [InlineData(null, "111111111", "test@email.com", "TestCountry", "TestCity", "TestStreet", "1/10")]
        [InlineData("TestDescription", "111111111", null, "TestCountry", "TestCity", "TestStreet", "1/10")]
        public async Task Update_ForInvalidModel_ReturnsBadRequest(string description, string contactNumber, string email, string country, string city, string street, string apartmentNumber)
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
            await Seed(restaurant);

            var command = new UpdateRestaurantCommand()
            {
                Description = description,
                ContactNumber = contactNumber,
                Email = email,
                Country = country,
                City = city,
                Street = street,
                ApartmentNumber = apartmentNumber
            };
            var httpContent = command.ToJsonHttpContent();
            //act

            var response = await _adminClient.PutAsync($"{_route}/{restaurant.Id}", httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Update_ForNotRestaueantOwner_ReturnsForbidden()
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
                CreatedById = 21345
            };

            await Seed(restaurant);

            var command = new UpdateRestaurantDto()
            {
                Description = "ValidTestDescription",
                ContactNumber = "111111111",
                Email = "Validtest@email.com",
                Country = "ValidTestCountry",
                City = "ValidTestCity",
                Street = "ValidTestStreet",
                ApartmentNumber = "1/10"
            };
            var httpContent = command.ToJsonHttpContent();
            //act

            var response = await _ownerClient.PutAsync($"{_route}/{restaurant.Id}", httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_ForRestaueantOwner_ReturnsOK()
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
                CreatedById = 2
            };

            await Seed(restaurant);

            var command = new UpdateRestaurantDto()
            {
                Description = "ValidTestDescription",
                ContactNumber = "111111111",
                Email = "Validtest@email.com",
                Country = "ValidTestCountry",
                City = "ValidTestCity",
                Street = "ValidTestStreet",
                ApartmentNumber = "1/10"
            };
            var httpContent = command.ToJsonHttpContent();
            //act

            var response = await _ownerClient.PutAsync($"{_route}/{restaurant.Id}", httpContent);
            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Update_ForValidModel_ReturnsOk()
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

            await Seed(restaurant);

            var command = new UpdateRestaurantDto()
            {
                Description = "ValidTestDescription",
                ContactNumber = "111111111",
                Email = "Validtest@email.com",
                Country = "ValidTestCountry",
                City = "ValidTestCity",
                Street = "ValidTestStreet",
                ApartmentNumber = "1/10"
            };
            var httpContent = command.ToJsonHttpContent();
            //act

            var response = await _adminClient.PutAsync($"{_route}/{restaurant.Id}", httpContent);
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


        private async Task Seed<T>(T obj) where T : class
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<OrderEatDbContext>();

            _dbContext.Add(obj);

            await _dbContext.SaveChangesAsync();
        }
    }
}