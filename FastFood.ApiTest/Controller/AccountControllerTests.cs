using FastFood.ApiTest.Authorization;
using FastFood.Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FastFood.ApiTest.Controller
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

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
                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                        services
                         .AddDbContext<FastFoodDbContext>(options => options.UseInMemoryDatabase("FastFoodDb"));
                    });
                });
            _client = _factory.CreateClient();
        }
    }
}