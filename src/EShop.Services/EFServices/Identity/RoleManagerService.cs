using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.DataLayer.Context;
using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using EShop.ViewModels.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EShop.Services.EFServices.Identity
{
    public class RoleManagerService(
        IRoleStoreService store,
        IEnumerable<IRoleValidator<Role>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<RoleManager<Role>> logger,
        IUnitOfWork uow) : RoleManager<Role>((RoleStoreService)store, roleValidators, keyNormalizer, errors, logger), IRoleManagerService
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly DbSet<Role> _roles = uow.Set<Role>();

        public async Task<List<ShowRoles>> GetRolesPreviewAsync() 
        {
            return await _roles.Select(r => new ShowRoles()
            {
                Id = r.Id,
                Name = r.Name,
                UsersCount = r.UserRole.Count
            }).ToListAsync();
        }

        public async Task<Role> RoleToDelete(int id)
        {
            return await _roles.FindAsync(id);
        }

         
    }
}