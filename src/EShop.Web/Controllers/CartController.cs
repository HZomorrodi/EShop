using Dto.Payment;
using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Entities.Identity;
using EShop.Services.Contracts;
using EShop.Services.EFServices;
using EShop.ViewModels.Application;
using EShop.ViewModels.Cart;
using EShop.Web.ViewComponents;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZarinPal.Class;

namespace EShop.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICartDetailService _cartDetailService;
        private readonly IProductService _productService;
        private readonly Payment _payment;
        private readonly Authority _authority;
        private readonly IUnitOfWork _uow;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"; // Sandbox key
        public CartController(ICartService cartService,
                                    ICartDetailService cartDetailService,
                                    IProductService productService,
                                    IHttpClientFactory httpClientFactory,
                                    IOptionsMonitor<StripeConfigsModel> optionsSnapshot,
                                    IUnitOfWork uow)
        {
            _cartService = cartService;
            _cartDetailService = cartDetailService;
            _productService = productService;
            var expose = new Expose();
            _payment = expose.CreatePayment();
            _authority = expose.CreateAuthority();
            _httpClientFactory = httpClientFactory;
            StripeConfiguration.ApiKey = optionsSnapshot.CurrentValue.SecretKey;
            _uow = uow;
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
            #region ZarinPal
            //var client = _httpClientFactory.CreateClient();

            //var requestData = new
            //{
            //    merchant_id = MerchantId,
            //    amount = userCart.TotalPrice + 15000, // 100,000 IRR
            //    description = "توضیحات سفارش",
            //    callback_url = Url.Action("PaymentResult", "Cart", new { area = "", orderId = userCart.Id }, protocol: Request.Scheme),
            //    email = "test@example.com",
            //    mobile = "09123456789"
            //};

            //string json = JsonConvert.SerializeObject(requestData);
            //StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            //HttpResponseMessage response = await client.PostAsync("https://sandbox.zarinpal.com/pg/v4/payment/request.json", content);
            //string responseJson = await response.Content.ReadAsStringAsync();

            //dynamic result = JsonConvert.DeserializeObject(responseJson);

            //if (result.data != null && result.data.code == 100)
            //{
            //    string authority = result.data.authority;
            //    return Redirect($"https://sandbox.zarinpal.com/pg/StartPay/{authority}");
            //}
            //else
            //{
            //    return Content("Error: " + responseJson);
            //}
            #endregion
            var domain = $"{Request.Scheme}://{Request.Host}";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = ["card"],
                LineItems =
            [
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = userCart.TotalPrice + 15000, // 20.00 USD
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Test Product"
                        }
                    },
                    Quantity = 1
                }
            ],
                Mode = "payment",
                SuccessUrl = $"{domain}/Cart/PaymentResult?session_id={{CHECKOUT_SESSION_ID}}&orderId={userCart.Id}",
                CancelUrl = domain + "/Cart/Cancel"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Redirect(session.Url);
        }

        public async Task<IActionResult> PaymentResult(int orderId, string status, string authority, string session_id)
        {
            //if (string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(authority))
            //    return View("Error2");
            if (string.IsNullOrWhiteSpace(session_id))
                return View("Error2");
            
            PaymentResultViewModel model = new();

            var sessionService = new SessionService();
            var session = sessionService.Get(session_id);

            // PaymentIntent ID
            string paymentIntentId = session.PaymentIntentId;

            // Get PaymentIntent details
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = paymentIntentService.Get(paymentIntentId);
            
            // Get Charge ID (if exists)
            //var chargeId = paymentIntent.Charges.Data.FirstOrDefault()?.Id;

            if (session.PaymentStatus == "paid")
            {
                Cart? userCart = await _cartService.FindByIdAsync(orderId);

                if (userCart == null)
                    return View("Error2");
                model.IsPay = true;
                model.TotalPrice = (userCart.TotalPrice + 15000).ToString("N0");
                model.RefId = paymentIntentId;
                userCart.IsPay = true;
                userCart.RefId = paymentIntentId;
                await _uow.SaveChangesAsync();
            }
            else
            {
                model.IsPay = false;
            }
            #region ZarinPal
            //if (string.Equals(status, "OK", StringComparison.OrdinalIgnoreCase))
            //{
            //    Cart? userCart = await _cartService.FindByIdAsync(orderId);
            //    if (userCart == null)
            //        return View("Error2");
            //    var client = _httpClientFactory.CreateClient();

            //    var verifyData = new
            //    {
            //        merchant_id = MerchantId,
            //        amount = 100000, // must match the request
            //        authority = authority
            //    };

            //    var json = JsonConvert.SerializeObject(verifyData);
            //    var content = new StringContent(json, Encoding.UTF8, "application/json");

            //    var response = await client.PostAsync("https://sandbox.zarinpal.com/pg/v4/payment/verify.json", content);
            //    var responseJson = await response.Content.ReadAsStringAsync();

            //    dynamic result = JsonConvert.DeserializeObject(responseJson);

            //    if (result.data != null && result.data.code == 100)
            //    {
            //        model.IsPay = true;
            //        //return Content($"✅ Payment successful! RefID: {result.data.ref_id}");
            //    }
            //    else
            //    {
            //        model.IsPay = false;
            //        //return Content("❌ Payment failed: " + responseJson);
            //    }
            //    //model.IsPay = verification.Status == 100;
            //    if (model.IsPay)
            //    {
            //        model.TotalPrice = (userCart.TotalPrice + 15000).ToString("#,0");
            //        model.RefId = result.data.ref_id;
            //        userCart.IsPay = true;
            //        userCart.RefId = result.data.ref_id;
            //        await _uow.SaveChangesAsync();
            //    }
            //    //else if (result?.data.code == 101)
            //    //{
            //    //    ViewBag.Message = "این صورتحساب قبلا تایید شده است.";
            //    //}

            //}
            #endregion
            return View(model);
        }
        public IActionResult Cancel()
        {
            return Content("Payment was canceled.");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId)
        {
            Entities.Product product = await _productService.FindByIdAsync(productId);
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
        [HttpPost]
        public async Task<IActionResult> IncreaseOrLowOffAsync(int productId, bool isIncrease, bool removeAll)
        {
            var product = await _productService.FindByIdAsync(productId);
            if (product is null)
                return BadRequest();
            var userId = User.Identity.GetUserId();
            var cartDetail = await _cartDetailService.GetCartDetailsBy(productId, userId);
            if (cartDetail is null)
                return BadRequest();
            if (removeAll)
            {
                _cartDetailService.Remove(cartDetail);
            }
            else if (isIncrease)
            {
                cartDetail.Count++;
            }
            else
            {
                if (cartDetail.Count <= 1)
                    _cartDetailService.Remove(cartDetail);
                else
                    cartDetail.Count--;
            }
            var userCart = await _cartService.GetUserCartAsync(userId);
            if (isIncrease)
            {
                userCart.TotalPrice = await _cartDetailService.CalculateUserCartTotalPriceAsync(userId)
                    + product.Price;
            }
            else
            {
                userCart.TotalPrice = await _cartDetailService.CalculateUserCartTotalPriceAsync(userId)
                                      - product.Price * (removeAll ? cartDetail.Count : 1);
            }
            await _uow.SaveChangesAsync();
            return Ok();
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
