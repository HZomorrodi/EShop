using EShop.DataLayer.Context;
using EShop.Services.Contracts.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Tls;
using Microsoft.EntityFrameworkCore;
using EShop.Entities.Identity;

namespace EShop.Services.EFServices.Identity
{
    public class UserStoreService(IUnitOfWork context,
        IdentityErrorDescriber? describer = null) :
        UserStore<User, Role, EShopDbContext, int,
            UserClaim, UserRole, UserLogin,
            UserToken, RoleClaim>
        ((EShopDbContext)context, describer),
        IUserStoreService
    {
    }
}
