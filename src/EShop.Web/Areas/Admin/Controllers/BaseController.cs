using EShop.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{IdentityRoleNames.Admin},{IdentityRoleNames.Customer}")]
    public class BaseController : Controller
    {
        
    }
}
