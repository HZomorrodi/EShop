using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.Contracts
{
    public interface ICookieManager
    {
        public void Add(string cookieName, string cookieValue, CookieOptions options = null);
        public string? GetValue(string cookieName);
    }
}
