using OrderEat.Domain.Interfaces;
using OrderEat.Infrastructure.Persistance;
using OrderEat.Infrastructure.Repositories;
using OrderEat.Infrastructure.Seeders;
using OrderEat.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OrderEat.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderEatDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("OrderEat")));

            services.AddScoped<OrderEatSeeder>();

            services.AddScoped<IUserContextService, UserContextService>();

            services.AddScoped<IAllergenRepository, AllergenRepository>();

            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddScoped<IRestaurantRepository, RestaurantRepository>();

            services.AddScoped<IDishRepository, DishRepository>();

            services.AddScoped<IIngredientRepository, IngredientRepository>();

            services.AddScoped<ISpecialDietRepository, SpecialDietRepository>();

            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}