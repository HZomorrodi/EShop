using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EShop.Services.EFServices.Identity
{
    public class SignInManagerService(IUserManagerService userManager,
          IHttpContextAccessor contextAccessor,
          IUserClaimsPrincipalFactory<User> claimsFactory,
          IOptions<IdentityOptions> optionsAccessor,
          ILogger<SignInManager<User>> logger,
          IAuthenticationSchemeProvider schemes,
          IUserConfirmation<User> confirmation) :
          SignInManager<User>((UserManagerService)userManager, contextAccessor,
              claimsFactory, optionsAccessor,
              logger, schemes, confirmation), ISignInManagerService
    {
    }
}
