using Colt.Application.Interfaces;
using Colt.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Colt.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();

            return services;
        }
    }
}
