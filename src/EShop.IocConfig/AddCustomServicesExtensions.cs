using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.Services.EFServices;
using EShop.ViewModels.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace EShop.IocConfig
{
    public static class AddCustomServicesExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            ConnectionStrings connectionStrings = serviceProvider.GetRequiredService<IOptionsMonitor<ConnectionStrings>>().CurrentValue;
            services.AddDbContext<EShopDbContext>(options => options.UseSqlServer(connectionStrings.EShopDbContextConnection));
            services.AddScoped<IUnitOfWork, EShopDbContext>();
            services.AddScoped<IProductService, ProductService>();
            services.AddIdentity<User, Role>().AddEntityFrameworkStores<EShopDbContext>().AddDefaultTokenProviders();
            return services;
        }
    }
}
