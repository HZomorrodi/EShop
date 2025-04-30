using EShop.Common.Constants;
using EShop.DataLayer.Context;
using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using EShop.ViewModels.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EShop.Web.Areas.Admin.Controllers
{
    [Area(AreaConstants.AdminArea)]
    public class RoleController : Controller
    {
        public RoleController(IRoleManagerService roleManagerService, IUnitOfWork uow)
        {
            RoleManagerService = roleManagerService;
            Uow = uow;
        }

        public IRoleManagerService RoleManagerService { get; }
        public IUnitOfWork Uow { get; }

        public async Task<IActionResult> Index()
        {
            return View(await RoleManagerService.GetRolesPreviewAsync());
        }
        [HttpPost]
        public IActionResult CheckRoleAccount()
        {
            return Json(true);
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                Role role = new()
                {
                    Name = model.Name
                };
                Microsoft.AspNetCore.Identity.IdentityResult result = await RoleManagerService.CreateAsync(role);
                if (result.Succeeded)
                {
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

        public async Task<IActionResult> Edit(int id)
        {
            Role? role = await RoleManagerService.FindByIdAsync(id.ToString());
            EditRoleViewModel model = new()
            {
                Name = role.Name,
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                Role? role = await RoleManagerService.FindByIdAsync(model.Id.ToString());
                if (role == null)
                {
                    return View(model);
                }
                role.Name = model.Name;
                Microsoft.AspNetCore.Identity.IdentityResult result = await RoleManagerService.UpdateAsync(role);
                if (result.Succeeded)
                {
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
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            Role role = await RoleManagerService.RoleToDelete(id);
            if (role == null)
            {
                return View("Error");
            }
            Microsoft.AspNetCore.Identity.IdentityResult result = await RoleManagerService.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View("Error");
            }
        }
    }
}
