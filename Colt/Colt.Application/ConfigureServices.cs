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
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IInvoiceService, InvoiceService>();

            return services;
        }
    }
}
