using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.Services.Contracts;
using EShop.Services.Contracts.WebApi;
using EShop.ViewModels.TestWebApi;
using EShop.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Threading.Tasks;

namespace EShop.Web.Controllers
{
    [TypeFilter(typeof(CustomAuthorize))]
    public class WebApiController(ICookieManager cookieManager, IUserServiceWebApi userServiceWebApi) : Controller
    {
        private readonly ICookieManager _cookieManager = cookieManager;
        private readonly IUserServiceWebApi _userServiceWebApi = userServiceWebApi;

        public async Task<IActionResult> IndexAsync()
        {
            OperationResult<List<ShowUserViewModel?>> result = await _userServiceWebApi.GetAllUserAsync();
            if (!result.IsSuccess)
            {
                return View("Error2");
            }
            return View(result.Result);
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAsync(AddUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", PublicConstantStrings.ModelStateErrorMessage);
                return View(model);
            }
            model.Avatar = await model.UserAvatar.ConvertToBase64();
            model.UserAvatar = null;
            var result = await _userServiceWebApi.AddAsync(model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", "نام کاربری تکراری است");
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    result = false,
                    message = PublicConstantStrings.ModelStateErrorMessage
                });
            }
            OperationResult<string> result = await _userServiceWebApi.Login(model);
            if (!result.IsSuccess)
            {
                return Json(new
                {
                    result = false,
                    message = "نام کاربری یا رمز عبور اشتباه است"
                });
            }
            else
            {
                _cookieManager.Add("JWTToken", result.Result.Trim('"'));
                return Json(new
                {
                    result = true
                });
            }
        }
    }
}
public class CodesMessage
{
    public const int DuplicateUserName = 10;

    public string GetMessage(int code)
    {
        var result = string.Empty;
        if (code == 10)
        {
            result = "نام کاربری تکراری است";
        }
        else if (code == 11)
        {

        }

        return result;
    }
}
