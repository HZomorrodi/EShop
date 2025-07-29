using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.Services.EFServices;
using EShop.ViewModels.Cart;
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
            if (userCart is null)
            {
                userCart = new()
                {
                    UserId = userId
                };
                await _cartService.AddAsync(userCart);
            }

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
            await _uow.SaveChangesAsync();
            return Json(userCart.TotalPrice.ToString("#,0"));
        }

        public async Task<string> GetUserCartTotalPrice()
        {
            int userId = User.Identity.GetUserId();
            return (await _cartDetailService.CalculateUserCartTotalPriceAsync(userId)).ToString("#,0");
        }

        public async Task<PartialViewResult> ShowCartDetailsPreview()
        {
            int userId = User.Identity.GetUserId();
            List<CartDetailPreviewViewModel> cartDetails = await _cartDetailService.GetCartDetailsByAsync(userId);
            string userCartTotalPrice = cartDetails.Sum(c => c.Price * c.Count).ToString("#,0"); ;
            ShowCartDetailsViewModel model = new()
            {
                CartDetails = cartDetails,
                UserCartTotalPrice = userCartTotalPrice,
            };
            return PartialView("_CartDetailsPartial", model);
        }
        public async Task<IActionResult> MyCarts()
        {
            int userId = User.Identity.GetUserId();
            List<ShowCartPreviewForClientViewModel> model = await _cartService.GetUserCartsForClient(userId);
            return View(model);
        }
        public async Task<IActionResult> ShowCartDetails(int id)
        {
            int userId = User.Identity.GetUserId();
            List<CartDetailPreviewViewModel> cartDetails = await _cartDetailService.GetCartDetailsAsync(userId, id);
            return View(cartDetails);
        }
    }
}
