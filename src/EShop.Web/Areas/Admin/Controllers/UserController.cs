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
    public class UserController(IUserManagerService userManagerService, IRoleManagerService roleManagerService) : BaseController
    {
        private readonly IUserManagerService _userManagerService = userManagerService;
        private readonly IRoleManagerService _roleManagerService = roleManagerService;

        public async Task<IActionResult> Index()
        {
            return View(await _userManagerService.GetUsersPreviewAsync());
        }
        public IActionResult Add()
        {
            ViewBag.SelectedRoles = _roleManagerService.Roles.Select(x => x.Name).ToList();
            return View(new AddUserViewModel());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            ViewBag.SelectedRoles = _roleManagerService.Roles.Select(x => x.Name).ToList();
            if (!await _roleManagerService.CheckRolesAsync(model.SelectedRoles))
            {
                return View("Error2");
            }
            if (ModelState.IsValid)
            {
                User user = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.UserName,
                    EmailConfirmed = true,
                    CreatedDateTime = DateTime.Now,
                    IsActive = true,
                };
                Microsoft.AspNetCore.Identity.IdentityResult result = await _userManagerService.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManagerService.AddToRolesAsync(user, model.SelectedRoles);
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

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.SelectedRoles = _roleManagerService.Roles.Select(x => x.Name).ToList();
            EditUserViewModel editUserViewModel = await _userManagerService.GetUsersForEditAsync(id);
            if (editUserViewModel is null)
                return View("NotFound");
            return View(editUserViewModel);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await _roleManagerService.CheckRolesAsync(model.SelectedRoles))
                    return View("Error2");

                User? user = await _userManagerService.FindByIdAsync(model.Id.ToString());
                if (user is null)
                    return View("NotFound");

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.IsActive = true;
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    user.PasswordHash = _userManagerService.PasswordHasher.HashPassword(user, model.Password);
                }
                Microsoft.AspNetCore.Identity.IdentityResult result = await _userManagerService.UpdateAsync(user);
                if (result.Succeeded)
                {
                    IList<string> roles = await _userManagerService.GetRolesAsync(user);
                    await _userManagerService.RemoveFromRolesAsync(user, roles);
                    await _userManagerService.AddToRolesAsync(user, model.SelectedRoles);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
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

        public async Task<IActionResult> ChangeUserStatusAsync(int id)
        {
            User? user = await _userManagerService.FindByIdAsync(id.ToString());
            if (user is null)
                return View("NotFound");
            user.IsActive = !user.IsActive;
            Microsoft.AspNetCore.Identity.IdentityResult result = await _userManagerService.UpdateAsync(user);
            await _userManagerService.UpdateSecurityStampAsync(user);
            if (!result.Succeeded)
                return View("Error2");
            return RedirectToAction(nameof(Index));
        }
    }
}
