using EShop.Common.Constants;
using EShop.Common.Mvc;
using EShop.Entities.Identity;
using EShop.Services.Contracts;
using EShop.Services.Contracts.Identity;
using EShop.Services.EFServices.Identity;
using EShop.ViewModels.Account;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;
using System.Security.Principal;
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
        private readonly ILogger<AccountController> _logger = logger;
        private readonly IUserManagerService _userManager = userManager;
        private readonly IViewRendererService _rendererService = viewRendererService;
        private readonly IEmailSenderService _emailSenderService = emailSenderService;
        private readonly ISignInManagerService _signInManagerService = signInManagerService;

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckUserName(string userName)
        {
            User? user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                return Json(true);
            return Json(false);
        }
        public async Task<IActionResult> CheckEmail(string email)
        {
            User? user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Json(true);
            return Json(false);
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
                CreatedDateTime = DateTime.Now,
                IsActive = true,
            };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation(LogCodes.RegisterCode, $"{user.UserName} creates a new account");
                string activationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //send Email
                string body = await _rendererService.RenderViewToStringAsync(
                    "~/Views/EmailTemplates/_ActivationUserEmailTemplate.cshtml",
                    new RegisterEmailConfirmationViewModel()
                    {
                        ActivationCode = activationCode,
                        UserName = model.UserName,
                        CreatedDateTime = user.CreatedDateTime.ToString()
                    });

                await _emailSenderService.SendEmailAsync(model.Email,
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
            LoginViewModel model = new()
            {
                ExternalLogins = [.. await _signInManagerService.GetExternalAuthenticationSchemesAsync()],
            };

            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            ViewData["returnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string? returnUrl, LoginViewModel model)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            ViewData["returnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", PublicConstantStrings.ModelStateErrorMessage);
                return View(model);
            }
            User? user = await _userManager.FindByNameAsync(model.UserName);
            if (user is null)
            {
                ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است");
            }
            else if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "ابتدا حساب کاربری خود را فعال کنید");
            }
            else if (!user.IsActive)
            {
                ModelState.AddModelError("", " حساب کاربری شما غیر فعال است");
            }
            else
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManagerService.PasswordSignInAsync
                    (user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(LogCodes.LoginCode, $"{user.UserName} logged in.");
                    IList<Claim> claims = await _userManager.GetClaimsAsync(user);
                    if (!claims.Any(c => c.Type == IdentityClaimNames.FullName))
                    {
                        await _userManager.AddClaimAsync(user, new Claim(IdentityClaimNames.FullName,
                              string.IsNullOrWhiteSpace(user.FullName) ? user.UserName! : user.FullName));
                    }
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
            if (User.Identity?.IsAuthenticated == true)
            {
                errors.Add("شما قبلا وارد سیستم شده اید");
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    errors.Add(PublicConstantStrings.ModelStateErrorMessage);
                    return BadRequest(errors);
                }
                User? user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                    errors.Add("نام کاربری یا رمز عبور اشتباه است");
                }
                else if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    errors.Add("ابتدا حساب کاربری خود را فعال کنید");
                }
                else if (!user.IsActive)
                {
                    errors.Add(" حساب کاربری شما غیر فعال است");
                }
                else
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManagerService.PasswordSignInAsync
                        (user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        IList<Claim> claims = await _userManager.GetClaimsAsync(user);
                        if (!claims.Any(c => c.Type == IdentityClaimNames.FullName))
                        {
                            await _userManager.AddClaimAsync(user, new Claim(IdentityClaimNames.FullName,
                                  string.IsNullOrWhiteSpace(user.FullName) ? user.UserName! : user.FullName));
                        }
                        //await signInManagerService.SignOutAsync();
                        //await signInManagerService.SignInAsync(user, model.RememberMe, "pwd");
                        _logger.LogInformation(LogCodes.LoginCode, $"{user.UserName} creates a new account");
                        return Json("Success");
                    }
                    errors.Add("نام کاربری یا رمز عبور اشتباه است");
                }
            }
            return BadRequest(errors);
        }

        public async Task<IActionResult> ConfirmationAccount(string code, string userName)
        {
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(userName))
            {
                return View("Error2");
            }
            User? user = await _userManager.FindByNameAsync(userName);
            if (user is null)
            {
                return View("NotFound");
            }
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, code);
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
            User? user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return View("ForgotPasswordConfirmation");
            }
            else if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "ابتدا حساب کاربری خود را فعال کنید");
                return View();
            }
            else
            {
                string resetPasswordCode = await _userManager.GeneratePasswordResetTokenAsync(user);
                string body = await _rendererService.RenderViewToStringAsync(
                    "~/Views/EmailTemplates/_ForgotPasswordEmailTemplate.cshtml",
                    new ForgotPasswordEmailViewModel()
                    {
                        UserName = user.UserName,
                        ResetPasswordCode = resetPasswordCode,
                    });

                await _emailSenderService.SendEmailAsync(model.Email,
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
            User? user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View("ResetPasswordConfirmation");
            }
            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
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
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Logout()
        {
            User? user = User.Identity?.IsAuthenticated == true ? await _userManager.GetUserAsync(User) : null;
            if (user is not null)
            {
                await _signInManagerService.SignOutAsync();
                await _userManager.UpdateSecurityStampAsync(user);
                _logger.LogInformation(LogCodes.LogoutCode, $"{user.UserName} logged out.");
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            if (returnUrl == "/Account/ConfirmationAccount")
            {
                returnUrl = "/";
            }
            string? redirectUrl = Url.Action(nameof(ExternalLoginCallBack), "Account", new { area = "", returnUrl });
            Microsoft.AspNetCore.Authentication.AuthenticationProperties properties =
                _signInManagerService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }
        public async Task<IActionResult> EditAccount()
        {
            string? currentUserId = User.Claims.SingleOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;
            bool succeed = int.TryParse(currentUserId, out int userId);
            EditAccountViewModel? model = null;
            if (succeed)
            {
                model = await _userManager.GetUserForEditAccountAsync(userId);
            }
            else if (!succeed || model is null)
            {
                return View("Error2");
            }
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(EditAccountViewModel model)
        {
            if (ModelState.IsValid) 
            {
                string? currentUserId = User.Claims.SingleOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;
                User? user = await userManager.FindByIdAsync(currentUserId);
                if (user is null)
                    return View("Error2");

                user.UserName = model.UserName;
                user.LastName = model.LastName;
                user.FirstName = model.FirstName;
                user.Email = model.Email;

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
                }
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _signInManagerService.RefreshSignInAsync(user).ConfigureAwait(false);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", PublicConstantStrings.ModelStateErrorMessage);
            }
            return View(model);
        }
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl, string remoteError)
        {
            ViewData["returnUrl"] = returnUrl;
            LoginViewModel model = new()
            {
                ExternalLogins = [.. await _signInManagerService.GetExternalAuthenticationSchemesAsync()],
            };
            if (remoteError is not null)
            {
                ModelState.AddModelError(string.Empty, $"Error : {remoteError}");
                return View(nameof(Login), model);
            }
            ExternalLoginInfo? externalLoginInfo = await _signInManagerService.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                ModelState.AddModelError(string.Empty, "خطایی به وجود آمد، مجددا تلاش نماید");
                return View(nameof(Login), model);
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManagerService.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, true, true);
            if (signInResult.Succeeded)
            {
                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction(nameof(HomeController.Index), "Home", new { area = string.Empty });
            }
            string? email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            if (email is null)
            {
                return View("Error2");
            }
            User? user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                user = new()
                {
                    UserName = Guid.NewGuid().ToString("N"),
                    Email = email,
                    IsActive = true,
                    EmailConfirmed = true,
                    CreatedDateTime = DateTime.Now,
                    UserClaims =
                    [
                        new()
                        {
                            ClaimType = IdentityClaimNames.FullName,
                            ClaimValue = "- - -"
                        }
                    ]
                };
                _logger.LogInformation(LogCodes.RegisterCode, $"{user.UserName} creates a new account");
                await _userManager.CreateAsync(user);
            }
            _logger.LogInformation(LogCodes.LoginCode, $"{user.UserName} logged in.");
            await _userManager.AddLoginAsync(user, externalLoginInfo);
            await _signInManagerService.SignInAsync(user, true);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(HomeController.Index), "Home", new { area = string.Empty });
        }

    }
}
