using EShop.Common;
using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.Services.EFServices;
using EShop.ViewModels.Categories;
using EShop.ViewModels.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShop.Web.Areas.Admin.Controllers
{
    [Area(AreaConstants.AdminArea)]
    public class ProductController(IProductService productService, ICategoryService categoryService, IUnitOfWork uow) : BaseController
    {
        private readonly IProductService productService = productService;
        private readonly ICategoryService categoryService = categoryService;
        private readonly IUnitOfWork uow = uow;
        public ActionResult Index()
        {
            return View(productService.GetProductsPreview());
        }

        public async Task<ActionResult> Add()
        {
            List<ShowCategory> categories = await categoryService.AllMainCategoriesAsync();
            ViewBag.MainCategories = categories.ToList().CreateSelectListItem(addChooseOneItem: false);
            return View(new AddProductViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", PublicConstantStrings.ModelStateErrorMessage);
                List<ShowCategory> categories = await categoryService.AllMainCategoriesAsync();
                ViewBag.MainCategories = categories.ToList().CreateSelectListItem(addChooseOneItem: false, selectedItem: model.CategoryId);
                return View(new AddProductViewModel());
            }
            Product product = new()
            {
                CategoryId = model.CategoryChildrenId,
                Title = model.Title,
                Price = model.Price,
                Description = model.Description,
            };
            foreach (var property in model.Properties)
            {
                string[] splittedProperty = property.Split("|||", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                product.ProductProperties.Add(new ProductProperty
                {
                    Title = splittedProperty[0],
                    Value = splittedProperty[1],
                });
            }
            foreach (var image in model.Images)
            {
                string imageExtension = Path.GetExtension(image.FileName);
                string imageName = Guid.NewGuid().ToString("N");
                image.SaveImage(imageName, imageExtension, "products");
                product.ProductImages.Add(new ProductImage
                {
                    Title = imageName + imageExtension,
                });
            }
            await productService.AddAsync(product);
            await uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Edit(int id)
        {
            EditProductViewModel? editModel = await productService.GetProductToEdit(id);
            if (editModel is null)
                return View("NotFound");
            List<ShowCategory> categories = await categoryService.AllMainCategoriesAsync();
            ViewBag.MainCategories = categories.ToList().CreateSelectListItem(addChooseOneItem: false, selectedItem: editModel.CategoryId);
            return View(editModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EditProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", PublicConstantStrings.ModelStateErrorMessage);
                List<ShowCategory> categories = await categoryService.AllMainCategoriesAsync();
                ViewBag.MainCategories = categories.ToList().CreateSelectListItem(addChooseOneItem: false, selectedItem: model.CategoryId);
                return View(new AddProductViewModel());
            }
            Product? product = await productService.GetProductToUpdateAsync(id);

            if (product?.ProductImages.Count == 0 && model?.Images?.Count == 0)
            {
                ModelState.AddModelError("", "لطفا حداقل یک عکس را برای محصول انتخاب کنید");
                List<ShowCategory> categories = await categoryService.AllMainCategoriesAsync();
                ViewBag.MainCategories = categories.ToList().CreateSelectListItem(addChooseOneItem: false, selectedItem: model.CategoryId);
                return View(new AddProductViewModel());
            }
            product.CategoryId = model.CategoryChildrenId;
            product.Title = model.Title;
            product.Price = model.Price;
            product.Description = model.Description;

            product.ProductProperties.Clear();
            foreach (var property in model.Properties)
            {
                string[] splittedProperty = property.Split("|||", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                product.ProductProperties.Add(new ProductProperty
                {
                    Title = splittedProperty[0],
                    Value = splittedProperty[1],
                });
            }

            foreach (var image in model.Images)
            {
                string imageExtension = Path.GetExtension(image.FileName);
                string imageName = Guid.NewGuid().ToString("N");
                image.SaveImage(imageName, imageExtension, "products");
                product.ProductImages.Add(new ProductImage
                {
                    Title = imageName + imageExtension,
                });
            }
            productService.Update(product);
            await uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<JsonResult> GetSubCategories(int mainCategoryId)
        {
            return Json(await categoryService.GetCategoryChildrenAsync(mainCategoryId));
        }

    }
}
