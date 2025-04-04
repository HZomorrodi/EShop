using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.DataLayer.Context;
using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EShop.Services.EFServices.Identity
{
    public class RoleStoreService(IUnitOfWork context, IdentityErrorDescriber? describer = null)
        : RoleStore<Role, EShopDbContext, int, UserRole, RoleClaim>((EShopDbContext)context, describer), IRoleStoreService
    {

    }
}