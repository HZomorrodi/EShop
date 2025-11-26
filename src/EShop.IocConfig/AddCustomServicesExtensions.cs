using EShop.Common.Mvc;
using EShop.DataLayer.Context;
using EShop.Entities.Identity;
using EShop.Services;
using EShop.Services.Contracts;
using EShop.Services.Contracts.Identity;
using EShop.Services.Contracts.WebApi;
using EShop.Services.EFServices;
using EShop.Services.EFServices.Identity;
using EShop.Services.EFServices.WebApi;
using EShop.ViewModels.App;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EShop.IocConfig
{
    public static class AddCustomServicesExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            ConnectionStringsModel connectionStrings = serviceProvider.GetRequiredService<IOptionsMonitor<ConnectionStringsModel>>().CurrentValue;
            services.AddDbContext<EShopDbContext>(options => options.UseSqlServer(connectionStrings.EShopDbContextConnection));

            #region RegisterIdentityServices

            services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimService>();
            services.AddScoped<UserClaimsPrincipalFactory<User, Role>, UserClaimService>();

            services.AddScoped<IRoleManagerService, RoleManagerService>();
            services.AddScoped<RoleManager<Role>, RoleManagerService>();

            services.AddScoped<IRoleStoreService, RoleStoreService>();
            services.AddScoped<RoleStore<Role, EShopDbContext, int, UserRole, RoleClaim>, RoleStoreService>();

            services.AddScoped<ISignInManagerService, SignInManagerService>();
            services.AddScoped<SignInManager<User>, SignInManagerService>();

            services.AddScoped<IUserManagerService, UserManagerService>();
            services.AddScoped<UserManager<User>, UserManagerService>();

            services.AddScoped<IUserStoreService, UserStoreService>();
            services.AddScoped<UserStore<User, Role, EShopDbContext, int,
                UserClaim, UserRole, UserLogin,
                UserToken, RoleClaim>, UserStoreService>();

            #endregion

            services.AddScoped<IUnitOfWork, EShopDbContext>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductImageService, ProductImageService>();
            services.AddScoped<IProductTagService, ProductTagService>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<ICartService, CartService>();    
            services.AddScoped<ICartDetailService, CartDetailService>();    
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<ICookieManager, CookieManager>();
            services.AddScoped<IUserServiceWebApi, UserServiceWebApi>();
            services.AddHttpClient<IHttpClientService, HttpClientService>(client =>
            {
                // Configure the HttpClient
                client.BaseAddress = new Uri("https://localhost:7002/");
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Add("User-Agent", "EShop-App");
            });

            services.AddIdentity<User, Role>(setupAction).
                //AddEntityFrameworkStores<EShopDbContext>().
                AddUserStore<UserStoreService>().
                AddRoleStore<RoleStoreService>().
                AddUserManager<UserManagerService>().
                AddRoleManager<RoleManagerService>().
                AddSignInManager<SignInManagerService>().
                AddDefaultTokenProviders();
            services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.Zero);
            services.AddAuthentication().AddGoogle(Options =>
            {
                Options.ClientSecret = "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";
                Options.ClientId = "860741161447-12k9seic3pgpi6gadj5f8t2115d39r5v.apps.googleusercontent.com";
            });
            services.AddRazorViewRenderer();
            return services;
        }

        private static Action<IdentityOptions> setupAction = identityOptions =>
           {
               identityOptions.Password.RequireDigit = false;
               identityOptions.Password.RequireLowercase = false;
               identityOptions.Password.RequireUppercase = false;
               identityOptions.Password.RequireNonAlphanumeric = false;

               identityOptions.Lockout.AllowedForNewUsers = false;
               identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
               identityOptions.Lockout.MaxFailedAccessAttempts = 3;

               identityOptions.SignIn.RequireConfirmedAccount = true;
               identityOptions.SignIn.RequireConfirmedEmail = true;
               identityOptions.SignIn.RequireConfirmedPhoneNumber = false;

               identityOptions.User.RequireUniqueEmail = true;
           };
        public static IServiceCollection AddRazorViewRenderer(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IViewRendererService, ViewRendererService>();
            return services;
        }
    }
}
