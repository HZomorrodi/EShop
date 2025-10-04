using EShop.DataLayer.Context;
using EShop.Entities.WebApi;
using EShop.Services.Contracts.Identity.WebApi;
using EShop.ViewModels.Users.WebApi;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.EFServices.Identity.WebApi
{
    public class UserService(IUnitOfWork uow) : GenericService<User>(uow), IUserService
    {
        private readonly DbSet<User> _user = uow.Set<User>();

        public async Task<UserToBuildJwtTokenViewModel?> GetUserBy(string userName, string password)
        {
            User? user = await _user.Include(u => u.Roles)
                .SingleOrDefaultAsync(u => u.UserName == userName && u.PassWord == password);
            if (user is null)
                return null;
            return new UserToBuildJwtTokenViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Roles = user.Roles.Select(r => r.Title).ToList(),
            };
        }

        public bool IsExistsByUserNameForAdd(string userName)
        {
            return _user.Any(u => u.UserName == userName);
        }
    }
}
