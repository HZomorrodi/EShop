using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        [Area(AreaConstants.AdminArea)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
