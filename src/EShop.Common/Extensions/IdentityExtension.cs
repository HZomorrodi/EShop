using EShop.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Common.Extensions
{
    public static class IdentityExtension
    {
        public static string? GetUserFullName(this IIdentity? identity)
        {
            ClaimsIdentity? claimsIdentity = identity as ClaimsIdentity;
            return claimsIdentity.FindFirstValue(IdentityClaimNames.FullName);
        }
        public static int GetUserId(this IIdentity? identity)
        {
            ClaimsIdentity? claimsIdentity = identity as ClaimsIdentity;
            return int.Parse(claimsIdentity.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        private static string? FindFirstValue(this ClaimsIdentity? claimsIdentity, string type)
        {
            return claimsIdentity?.FindFirst(type)?.Value;
        }
    }
}
