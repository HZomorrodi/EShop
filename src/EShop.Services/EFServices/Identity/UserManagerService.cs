using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EShop.Services.EFServices.Identity
{
    public class UserManagerService(IUserStoreService store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<User> passwordHasher,
        IEnumerable<IUserValidator<User>> userValidators,
        IEnumerable<IPasswordValidator<User>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<User>> logger) :
        UserManager<User>((UserStoreService)store, optionsAccessor, passwordHasher,
            userValidators, passwordValidators, keyNormalizer,
            errors, services, logger), IUserManagerService
    {
    }
}
