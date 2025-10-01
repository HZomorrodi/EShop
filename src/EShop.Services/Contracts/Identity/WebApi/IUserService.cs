using EShop.Entities.WebApi;
using EShop.ViewModels.Users.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.Contracts.Identity.WebApi
{
    public interface IUserService:IGenericService<User>
    {
        Task<UserToBuildJwtTokenViewModel?> GetUserBy(string userName, string password);
    }
}
