using FastFood.Infrastructure.Persistance;

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
        }
    }
}