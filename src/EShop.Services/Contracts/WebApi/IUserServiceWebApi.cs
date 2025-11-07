using EShop.ViewModels.TestWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.Contracts.WebApi
{
    public interface IUserServiceWebApi
    {
        public Task<OperationResult<List<ShowUserViewModel?>>> GetAllUserAsync();
        public Task<OperationResult<string>> Login(LoginViewModel input);
        public Task<OperationResult<string>> AddAsync(AddUserViewModel input);
    }
}
