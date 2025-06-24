using EShop.Common;
using EShop.Common.Constants;
using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.Services.EFServices;
using EShop.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShop.Web.Areas.Admin.Controllers
{
    [Area(AreaConstants.AdminArea)]
    public class CategoryController(ICategoryService categoryService, IUnitOfWork uow) : BaseController
    {
        private readonly ICategoryService categoryService = categoryService;
        private readonly IUnitOfWork _uow = uow;

        public async Task<IActionResult> Index()
        {
            List<ShowCategory> categories = await categoryService.AllMainCategoriesAsync();
            return View(categories);
        }
        public async Task<IActionResult> Add()
        {
            List<ShowCategory> categories = await categoryService.AllMainCategoriesAsync();
            ViewBag.MainCategories = categories.ToList().CreateSelectListItem(firstItemText: "خودش سر دسته باشد");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await categoryService.AllMainCategoriesAsync();
                ViewBag.MainCategories = categories.ToList()
                    .CreateSelectListItem(model.ParentId, firstItemText: "خودش سر دسته باشد");
                ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
                return View(model);
            }
            await categoryService.AddAsync(new Category()
            {
                Title = model.Title,
                ParentId = model.ParentId == 0 ? null : model.ParentId
            });
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var categoryToDelete = categoryService.GetToDelete(id);
            if (categoryToDelete != null)
            {
                categoryService.Remove(categoryToDelete);
                await _uow.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var categories = await categoryService.AllMainCategoriesAsync(id);
            var category = await categoryService.FindByIdAsync(id);
            ViewBag.MainCategories = categories.ToList()
                .CreateSelectListItem(category.ParentId, firstItemText: "خودش سر دسته باشد");
            var editCatViewModel = new EditCategoryViewModel()
            {
                Id = category.Id,
                ParentId = category.ParentId,
                Title = category.Title
            };
            return View(editCatViewModel);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await categoryService.AllMainCategoriesAsync(model.Id);
                ViewBag.MainCategories = categories.ToList()
                    .CreateSelectListItem(model.ParentId, firstItemText: "خودش سر دسته باشد");
                ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
                return View(model);
            }

            if (model.Id == model.ParentId)
                return View("Error");
            categoryService.Update(new Category
            {
                Id = model.Id,
                Title = model.Title,
                ParentId = model.ParentId == 0 ? null : model.ParentId,
            });
            _uow.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ShowCategoryChildren(int mainCatId)
        {
            var categories = await categoryService.GetCategoryChildrenAsync(mainCatId);
            return View("_ShowCategoryeChildrenPartial", categories);
        }

    }
}
