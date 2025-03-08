using EShop.Common.Constants;
using EShop.Common.Mvc;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.ViewModels.Account;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(ILogger<AccountController> logger,
            UserManager<User> userManager,
            IViewRendererService viewRendererService,
            IEmailSenderService emailSenderService)
        {
            Logger = logger;
            UserManager = userManager;
            ViewRendererService = viewRendererService;
            EmailSenderService = emailSenderService;
        }

        public ILogger<AccountController> Logger { get; }
        public UserManager<User> UserManager { get; }
        public IViewRendererService ViewRendererService { get; }
        public IEmailSenderService EmailSenderService { get; }

        [HttpPost]
        public IActionResult CheckUserAccount(string UserName)
        {
            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            List<string> errors = new List<string>();
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    CreatedDateTime = DateTime.Now
                };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    Logger.LogInformation(LogCodes.RegisterCode, $"{user.UserName} creates a new account");
                    var activationCode = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    //send Email
                    string body = await ViewRendererService.RenderViewToStringAsync(
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
            }
            return BadRequest(errors);
        }

        public IActionResult ConfirmationAccount(string code, string UserName)
        {
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(UserName))
            {
                return View("Error2");
            }
            return StatusCode(400, new { Id3 = 5 });
        }

    }
}
