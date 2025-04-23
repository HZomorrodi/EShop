using EShop.Common.Constants;
using EShop.Common.Mvc;
using EShop.Entities.Identity;
using EShop.Services.Contracts;
using EShop.Services.Contracts.Identity;
using EShop.ViewModels.Account;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EShop.Web.Controllers
{
    public class AccountController(ILogger<AccountController> logger,
        IUserManagerService userManager,
        IViewRendererService viewRendererService,
        IEmailSenderService emailSenderService,
        ISignInManagerService signInManagerService) : Controller
    {
        public ILogger<AccountController> Logger { get; } = logger;
        public IUserManagerService UserManager { get; } = userManager;
        public IViewRendererService RendererService { get; } = viewRendererService;
        public IEmailSenderService EmailSenderService { get; } = emailSenderService;
        public ISignInManagerService SignInManagerService { get; } = signInManagerService;

        [HttpPost]
        public IActionResult CheckUserAccount(string UserName)
        {
            return Json(true);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            List<string> errors = [];
            if (!ModelState.IsValid)
            {
                errors.Add(PublicConstantStrings.ModelStateErrorMessage);
                return BadRequest(errors);
            }
            User user = new()
            {
                UserName = model.UserName,
                Email = model.Email,
                CreatedDateTime = DateTime.Now
            };
            IdentityResult result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                Logger.LogInformation(LogCodes.RegisterCode, $"{user.UserName} creates a new account");
                string activationCode = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                //send Email
                string body = await RendererService.RenderViewToStringAsync(
                    "~/Views/EmailTemplates/_ActivationUserEmailTemplate.cshtml",
                    new RegisterEmailConfirmationViewModel()
                    {
                        ActivationCode = activationCode,
                        UserName = model.UserName,
                        CreatedDateTime = user.CreatedDateTime.ToString()
                    });

                await EmailSenderService.SendEmailAsync(model.Email,
                    "فعال‌سازی حساب کاربری", body);

                return Json("Success");
            }
            foreach (IdentityError error in result.Errors)
            {
                errors.Add(error.Description);
            }
            return BadRequest(errors);
        }

        public async Task<IActionResult> Login(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string? returnUrl, LoginViewModel model)
        {
            ViewData["returnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", PublicConstantStrings.ModelStateErrorMessage);
                return View(model);
            }
            User user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است");
            }
            else if (!await UserManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "ابتدا حساب کاربری خود را فعال کنید");
            }
            else
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await SignInManagerService.PasswordSignInAsync
                    (user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    Logger.LogInformation(LogCodes.LoginCode, $"{user.UserName} logged in.");
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است");
            }
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> NaveBarLogin(LoginViewModel model)
        {
            List<string> errors = [];
            if (!ModelState.IsValid)
            {
                errors.Add(PublicConstantStrings.ModelStateErrorMessage);
                return BadRequest(errors);
            }
            User user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                errors.Add("نام کاربری یا رمز عبور اشتباه است");
            }
            else if (!await UserManager.IsEmailConfirmedAsync(user))
            {
                errors.Add("ابتدا حساب کاربری خود را فعال کنید");
            }
            else
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await SignInManagerService.PasswordSignInAsync
                    (user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    Logger.LogInformation(LogCodes.LoginCode, $"{user.UserName} creates a new account");
                    return Json("Success");
                }
                errors.Add("نام کاربری یا رمز عبور اشتباه است");
            }
            return BadRequest(errors);
        }

        public async Task<IActionResult> ConfirmationAccount(string code, string userName)
        {
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(userName))
            {
                return View("Error2");
            }
            User? user = await UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return View("NotFound");
            }
            IdentityResult result = await UserManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? nameof(ConfirmationAccount) : "Error2");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", PublicConstantStrings.ModelStateErrorMessage);
                return View(model);
            }
            User? user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View("ForgotPasswordConfirmation");
            }
            else if (!await UserManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "ابتدا حساب کاربری خود را فعال کنید");
                return View();
            }
            else
            {
                string resetPasswordCode = await UserManager.GeneratePasswordResetTokenAsync(user);
                string body = await RendererService.RenderViewToStringAsync(
                    "~/Views/EmailTemplates/_ForgotPasswordEmailTemplate.cshtml",
                    new ForgotPasswordEmailViewModel()
                    {
                        UserName = user.UserName,
                        ResetPasswordCode = resetPasswordCode,
                    });

                await EmailSenderService.SendEmailAsync(model.Email,
                    "باز نشانی روز عبور", body);
                return View("ForgotPasswordConfirmation");
            }
        }

        public IActionResult ResetPassword(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return View("Error2");
            }
            ViewData["Token"] = code;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            ViewData["Token"] = model.Token;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", PublicConstantStrings.ModelStateErrorMessage);
                return View(model);
            }
            User user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View("ResetPasswordConfirmation");
            }
            IdentityResult result = await UserManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        public PartialViewResult LoadLoginPartial()
        {
            return PartialView("_LoginPartial");
        }
        public PartialViewResult LoadRegisterPartial()
        {
            return PartialView("_RegisterPartial");
        }

    }
}
