using EShop.Entities.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.Contracts.Identity.WebApi
{
    public interface IRoleService : IGenericService<Role>
    {
        List<Role> GetRolesBy(List<string> roles);
    }
}
