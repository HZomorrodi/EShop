using Dto.Payment;
using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Entities.Identity;
using EShop.Services.Contracts;
using EShop.Services.EFServices;
using EShop.ViewModels.Cart;
using EShop.Web.ViewComponents;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZarinPal.Class;

namespace EShop.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICartDetailService _cartDetailService;
        private readonly IProductService _productService;
        private readonly Payment _payment;
        private readonly Authority _authority;
        private readonly IUnitOfWork _uow;

        public CartController(ICartService cartService,
                                    ICartDetailService cartDetailService,
                                    IProductService productService,
                                    IUnitOfWork uow)
        {
            _cartService = cartService;
            _cartDetailService = cartDetailService;
            _productService = productService;
            
            _uow = uow;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Checkout()
        {
            int userId = User.Identity.GetUserId();
            CheckoutViewModel model = new()
            {
                UserCartTotalPrice = await _cartDetailService.CalculateUserCartTotalPriceAsync(userId),
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var userId = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", PublicConstantStrings.ModelStateErrorMessage);
                return View(model);
            }
            Cart? userCart = await _cartService.GetUserCartAsync(userId);
            if (userCart is null || userCart.TotalPrice <= 0)
                return RedirectToAction(nameof(HomeController.Index), "Home");
            userCart.Address = model.Address;
            await _uow.SaveChangesAsync();
            Dto.Response.Payment.Request result = await _payment.Request(new DtoRequest()
            {
                Mobile = "09111111182",
                Description = "Description",
                Email = "Khi@gmail.com",
                MerchantId = "er",
                Amount = userCart.TotalPrice + 15000,
                CallbackUrl = Url.Action(nameof(PaymentResult), "Cart", new { area = "", orderId = userCart.Id }, protocol: Request.Scheme),
            }
            , Payment.Mode.sandbox);
            if (result.Status == 100)
            {
                return Redirect($"https://sandbox.zarinpal.com/pg/StartPay/{result.Authority}");
            }
            return View("Error2");
        }

        public async Task<IActionResult> PaymentResult(int orderId, string status, string authority)
        {
            if (string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(authority))
                return View("Error2");
            PaymentResultViewModel model = new();
            if (string.Equals(status, "OK", StringComparison.OrdinalIgnoreCase))
            {
                Cart? userCart = await _cartService.FindByIdAsync(orderId);
                if (userCart == null)
                    return View("Error2");
                Dto.Response.Payment.Verification verification = await _payment.Verification(new DtoVerification()
                {
                    Amount = userCart.TotalPrice + 15000,
                    MerchantId = "er",
                    Authority = authority,
                }
                , Payment.Mode.sandbox);
                model.IsPay = verification.Status == 100;
                if (verification.Status == 100)
                {
                    model.TotalPrice = (userCart.TotalPrice + 15000).ToString("#,0");
                    model.RefId = verification.RefId;
                    userCart.IsPay = true;
                    userCart.RefId = verification.RefId;
                    await _uow.SaveChangesAsync();
                }
                else if (verification.Status == 101)
                {
                    ViewBag.Message = "این صورتحساب قبلا تایید شده است.";
                }

            }
            return View(model);
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
            int userCartTotalPrice = cartDetails.Sum(c => c.Price * c.Count); 
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
