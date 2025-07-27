using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.Services.EFServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShop.Web.Controllers
{
    public class CartController(ICartService cartService,
                                ICartDetailService cartDetailService,
                                IProductService productService,
                                IUnitOfWork uow) : Controller
    {
        private readonly ICartService _cartService = cartService;
        private readonly ICartDetailService _cartDetailService = cartDetailService;
        private readonly IProductService _productService = productService;
        private readonly IUnitOfWork _uow = uow;

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId)
        {
            Product product = await _productService.FindByIdAsync(productId);
            if (product == null)
                return BadRequest();
            int userId = User.Identity.GetUserId();
            Cart? userCart = await _cartService.GetUserCartAsync(userId);
            userCart ??= new()
            {
                UserId = userId,
            };
            CartDetail? cartDetail = await _cartDetailService.GetCartDetailsBy(productId, userId);
            if (cartDetail is null)
            {
                userCart.CartDetails.Add(new()
                {
                    ProductId = productId,
                    Count = 1,
                    Price = product.Price,
                });
            }
            else
            {
                cartDetail.Count++;
            }
            userCart.TotalPrice += product.Price;
            await _cartService.AddAsync(userCart);
            await _uow.SaveChangesAsync();
            return Json(userCart.TotalPrice.ToString("#,0"));
        }
    }
}
