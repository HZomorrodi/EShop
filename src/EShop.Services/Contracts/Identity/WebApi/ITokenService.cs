using EShop.ViewModels.Users.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.Contracts.Identity.WebApi
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, UserToBuildJwtTokenViewModel user, bool rememberMe);
    }
}
