using EShop.DataLayer.Context;
using EShop.Entities.WebApi;
using EShop.Services.Contracts.Identity.WebApi;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.EFServices.Identity.WebApi
{
    public class RoleService(IUnitOfWork uow) : GenericService<Role>(uow), IRoleService
    {
        private readonly DbSet<Role> _role = uow.Set<Role>();

        public List<Role> GetRolesBy(List<string> roles)
        {
            return _role.Where(r => roles.Contains(r.Title)).ToList();
        }
    }
}
