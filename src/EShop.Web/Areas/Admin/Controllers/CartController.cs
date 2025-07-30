using EShop.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShop.Web.Areas.Admin.Controllers
{
    [Area(AreaConstants.AdminArea)]
    public class CartController(ICartService cartService, ICartDetailService cartDetailService) : BaseController
    {
        private readonly ICartService _cartService = cartService;
        private readonly ICartDetailService _cartDetailService = cartDetailService;

        public async Task<IActionResult> Index()
        {
            return View(await _cartService.GetUserCartsForAdmin());
        }
        public async Task<IActionResult> ShowCartDetailsAsync(int id)
        {
            return View(await _cartDetailService.GetCartDetailsForAdminAsync(id));
        }
    }
}
