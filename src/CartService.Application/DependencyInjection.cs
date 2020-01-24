using AutoMapper;
using CartService.Application.Mappings;
using CartService.Application.Repositories;
using CartService.Application.Services;
using CartService.Application.Services.Background;
using CartService.Application.Services.Customer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CartService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddTransient<IShoppingCartItemRepository, ShoppingCartItemRepository>();

            services.AddTransient<ICustomerShoppingCartService, CustomerShoppingCartService>();
            services.AddTransient<IBackgroundShoppingCartService, BackgroundShoppingCartService>();

            services.AddAutoMapper(
                cfg => cfg.AddProfile<AutomapperMappingProfile>(),
                AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
