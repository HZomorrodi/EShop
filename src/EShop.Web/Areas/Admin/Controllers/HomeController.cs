using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area(AreaConstants.AdminArea)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
