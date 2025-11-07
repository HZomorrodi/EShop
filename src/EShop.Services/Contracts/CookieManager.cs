using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.Contracts
{
    public class CookieManager : ICookieManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void Add(string cookieName, string cookieValue, CookieOptions options = null)
        {
            if (options is not null)
                _httpContextAccessor?.HttpContext?.Response.Cookies.Append(cookieName, cookieValue, options);
            else
                _httpContextAccessor?.HttpContext?.Response.Cookies.Append(cookieName, cookieValue);
        }

        public string? GetValue(string cookieName)
        {
            var cookies = _httpContextAccessor?.HttpContext?.Request.Cookies;
            return cookies?.TryGetValue(cookieName, out string? value) == true ? value : null;
        }
    }
}
