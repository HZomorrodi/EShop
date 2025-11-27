using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.Services.Contracts;
using EShop.Services.Contracts.WebApi;
using EShop.ViewModels.TestWebApi;
using EShop.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

        [AllowAnonymous]
        public IActionResult Login2()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login2(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error2");
            }
            SqlConnectionStringBuilder builder = new()
            {
                DataSource = ".",
                InitialCatalog = "TicketDb",
                IntegratedSecurity = true,
                MultipleActiveResultSets = true,
                TrustServerCertificate = true,
            };
            string commandText = $"SELECT TOP(1) * FROM Users WHERE [UserName] = N'{model.UserName}'" +
                $" AND [Password] = N'{@model.Password}'";
            string commandText2 = $"SELECT TOP(1) * FROM Users WHERE [UserName] = @UserName " +
                $"AND [Password] = @Password";
            using (SqlConnection connection = new(builder.ConnectionString))
            using (SqlCommand command = new(commandText2, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter ("@UserName", model.UserName));
                command.Parameters.Add(new SqlParameter("@Password", model.Password));
                SqlDataReader results = command.ExecuteReader();
                if (results.Read())
                {
                    var avatar = results["Avatar"];
                }
            }
            return View();
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
