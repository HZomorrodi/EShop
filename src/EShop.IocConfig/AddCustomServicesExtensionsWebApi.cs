using EShop.DataLayer.Context;
using EShop.Entities.Identity;
using EShop.Services;
using EShop.Services.Contracts;
using EShop.Services.Contracts.Identity;
using EShop.Services.Contracts.Identity.WebApi;
using EShop.Services.EFServices;
using EShop.Services.EFServices.Identity;
using EShop.Services.EFServices.Identity.WebApi;
using EShop.ViewModels.App;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.IocConfig
{
    public static class AddCustomServicesExtensionsWebApi
    {

        public static IServiceCollection AddCustomServicesWebApi(this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            ConnectionStringsModel connectionStrings = serviceProvider.GetRequiredService<IOptionsMonitor<ConnectionStringsModel>>().CurrentValue;
            services.AddDbContext<TicketDbContext>(options => options.UseSqlServer(connectionStrings.TicketDbContextConnection));
            services.AddScoped<IUnitOfWork, TicketDbContext>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
