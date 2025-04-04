using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EShop.Services.EFServices.Identity
{
    public class RoleManagerService(IRoleStoreService store,
        IEnumerable<IRoleValidator<Role>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<RoleManager<Role>> logger) :
        RoleManager<Role>((RoleStoreService)store, roleValidators, keyNormalizer,
            errors, logger), IRoleManagerService
    {
    }
}
