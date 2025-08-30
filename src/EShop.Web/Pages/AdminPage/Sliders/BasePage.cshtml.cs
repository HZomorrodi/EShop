using EShop.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EShop.Web.Pages.AdminPage.Sliders
{
    [Authorize(Roles = $"{IdentityRoleNames.Admin}")]
    public class BasePageModel : PageModel
    {
        
    }
}
