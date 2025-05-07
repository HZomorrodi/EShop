using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.DataLayer.Context;
using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using EShop.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        ILogger<UserManager<User>> logger,
        IUnitOfWork uow) :
        UserManager<User>((UserStoreService)store, optionsAccessor, passwordHasher,
            userValidators, passwordValidators, keyNormalizer,
            errors, services, logger), IUserManagerService
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly DbSet<User> _users = uow.Set<User>();
        public async Task<List<ShowUsersViewModel>> GetUsersPreviewAsync()
        {
            return await _users.Select(u => new ShowUsersViewModel()
            {
                UserName = u.UserName,
                CreatedDateTime = u.CreatedDateTime,
                FullName = u.FullName,
                IsActvie = u.IsActvie,
            }).ToListAsync();
        }

        public Task<User> UserToDelete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
