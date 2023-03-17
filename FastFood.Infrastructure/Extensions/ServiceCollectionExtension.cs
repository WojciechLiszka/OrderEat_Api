using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Persistance;
using FastFood.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FastFood.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FastFoodDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("FastFood")));

            services.AddScoped<IAllergenRepository, AllergenRepository>();

            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddScoped<IRestaurantRepository, RestaurantRepository>();

            services.AddScoped<IDishRepository, DishRepository>(); 

            services.AddScoped<IIngredientRepository, IngredientRepository>();
        }
    }
}