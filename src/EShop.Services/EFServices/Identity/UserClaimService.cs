using EShop.Common.Constants;
using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.EFServices.Identity
{
    public class UserClaimService(IUserManagerService userManager, IRoleManagerService roleManager, IOptions<IdentityOptions> options) :
        UserClaimsPrincipalFactory<User, Role>((UserManagerService)userManager, (RoleManagerService)roleManager, options)
    {
        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            ClaimsPrincipal principal = await base.CreateAsync(user);
            ((ClaimsIdentity)principal.Identity).AddClaims([new Claim(IdentityClaimNames.FullName, user.FullName)]);
            return principal;
        }
    }
}
