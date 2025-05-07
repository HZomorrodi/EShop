using EShop.Common.Constants;
using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using EShop.Services.EFServices.Identity;
using EShop.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace EShop.Web.Areas.Admin.Controllers
{
    [Area(AreaConstants.AdminArea)]
    public class UserController(IUserManagerService userManagerService, IRoleManagerService roleManagerService) : Controller
    {
        private readonly IUserManagerService _userManagerService = userManagerService;
        private readonly IRoleManagerService _roleManagerService = roleManagerService;

        public async Task<IActionResult> Index()
        {
            return View(await _userManagerService.GetUsersPreviewAsync());
        }
        public async Task<IActionResult> Add()
        {
            ViewBag.SelectedRoles = _roleManagerService.Roles.Select(x => x.Name).ToList();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    CreatedDateTime = DateTime.Now,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IsActvie = true,
                };
                Microsoft.AspNetCore.Identity.IdentityResult result = await _userManagerService.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                ModelState.AddModelError("", PublicConstantStrings.ModelStateErrorMessage);
            }
            return View(model);
        }
        public ActionResult CheckUserName() 
        {
            return Json(true);
        }
    }
}
