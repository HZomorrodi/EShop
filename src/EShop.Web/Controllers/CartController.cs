using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Add(int id)
        {
            return Json(true);
        }
    }
}
