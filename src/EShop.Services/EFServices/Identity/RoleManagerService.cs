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

        public async Task<List<ShowRolesViewModel>> GetRolesPreviewAsync()
        {
            return await _roles.Select(r => new ShowRolesViewModel()
            {
                Id = r.Id,
                Title = r.Name,
                UsersCount = r.UserRole.Count
            }).ToListAsync();
        }

        public async Task<Role?> RoleToDelete(int id)
        {
            return await _roles.Where(r => !r.UserRole.Any()).SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> CheckRolesAsync(List<string> SelectedRoles)
        {
            int validItemCount = await _roles.CountAsync(r => SelectedRoles.Contains(r.Name));
            return SelectedRoles.Count == validItemCount;
        }
    }
}