using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
