using EShop.Common.Constants;
using EShop.Common.Mvc;
using EShop.Entities.Identity;
using EShop.Services.Contracts;
using EShop.Services.Contracts.Identity;
using EShop.ViewModels.Account;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Controllers
{
    public class AccountController(ILogger<AccountController> logger,
        IUserManagerService userManager,
        IViewRendererService viewRendererService,
        IEmailSenderService emailSenderService) : Controller
    {
        public ILogger<AccountController> Logger { get; } = logger;
        public IUserManagerService UserManager { get; } = userManager;
        public IViewRendererService RendererService { get; } = viewRendererService;
        public IEmailSenderService EmailSenderService { get; } = emailSenderService;

        [HttpPost]
        public IActionResult CheckUserAccount(string UserName)
        {
            return Json(true);
        }

        [HttpPost]
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

        public IActionResult ConfirmationAccount(string code, string userName)
        {
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(userName))
            {
                return View("Error2");
            }
            return StatusCode(400, new { Id3 = 5 });
        }

    }
}
