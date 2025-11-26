using EShop.Common;
using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.DataLayer.Migrations;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.Services.EFServices;
using EShop.ViewModels.Categories;
using EShop.ViewModels.Products;
using EShop.ViewModels.ProductTags;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace EShop.Web.Areas.Admin.Controllers
{
    [Area(AreaConstants.AdminArea)]
    public class ProductController(IProductService productService,
                                   IProductTagService productTagService,
                                   ICategoryService categoryService,
                                   IProductImageService productImageService,
                                   IUnitOfWork uow) : BaseController
    {
        private readonly IProductService productService = productService;
        private readonly IProductTagService productTagService = productTagService;
        private readonly ICategoryService categoryService = categoryService;
        private readonly IProductImageService productImageService = productImageService;
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
                return View(model);
            }
            List<string>? productTags = [];
            if (model.Tags is not null)
            {
                List<TagifyValueViewModel>? convertedTags = JsonConvert.DeserializeObject<List<TagifyValueViewModel>>(model.Tags);
                productTags = convertedTags?.Where(x => x.Value is not null)
                   .Select(x => x.Value.Trim())
                   .Distinct()
                   .ToList();
                if (productTags.Count > 10 || productTags.Any(x => x.Length > 100))
                {
                    return View("Error2");
                }
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
            if (productTags?.Count > 0)
            {
                List<Entities.ProductTag> tags = productTagService.GetTags(productTags);
                productTags.ForEach(productTag =>
                {
                    Entities.ProductTag? addedTag = tags.SingleOrDefault(tag => tag.Title == productTag);
                    product.ProductProductTags.Add(new ProductProductTag { ProductTag = addedTag ?? new Entities.ProductTag { Title = productTag } });
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
                return View(model);
            }
            List<string>? productTags = [];
            if (model.SelectedTags is not null)
            {
                List<TagifyValueViewModel>? convertedTags = JsonConvert.DeserializeObject<List<TagifyValueViewModel>>(model.SelectedTags);
                productTags = convertedTags?.Where(x => x.Value is not null)
                   .Select(x => x.Value.Trim())
                   .Distinct()
                   .ToList();
                if (productTags.Count > 10 || productTags.Any(x => x.Length > 100))
                {
                    return View("Error2");
                }
            }
            Product? product = await productService.GetProductToUpdateAsync(id);
            if (product?.ProductImages.Count == 0 && model?.Images?.Count == 0)
            {
                ModelState.AddModelError(nameof(EditProductViewModel.Images), "لطفا حداقل یک عکس را برای محصول انتخاب کنید");
                List<ShowCategory> categories = await categoryService.AllMainCategoriesAsync();
                ViewBag.MainCategories = categories.ToList().CreateSelectListItem(addChooseOneItem: false, selectedItem: model.CategoryId);
                return View(model);
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
            product.ProductProductTags.Clear();
            if (productTags?.Count > 0)
            {
                List<Entities.ProductTag> tags = productTagService.GetTags(productTags);
                productTags.ForEach(productTag =>
                {
                    Entities.ProductTag? addedTag = tags.SingleOrDefault(tag => tag.Title == productTag);
                    product.ProductProductTags.Add(new ProductProductTag { ProductTag = addedTag ?? new Entities.ProductTag { Title = productTag } });
                });
            }
            productService.Update(product);
            await uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var productToDelete = await productService.GetProductToDelete(id);
            if (productToDelete is null)
                return View("Error2");
            productService.Remove(productToDelete);
            await uow.SaveChangesAsync();
            foreach (var image in productToDelete.ProductImages)
            {
                WorkWithImages.RemoveImage(image.Title, "products");
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveProductPicture(string imageName)
        {
            ProductImage? productImage = await productImageService.RemoveProductImageByNameAsync(imageName);
            if (productImage is null)
            {
                return Json(false);
            }
            else
            {
                WorkWithImages.RemoveImage(imageName, "products");
                productImageService.Remove(productImage);
                await uow.SaveChangesAsync();
                return Json(true);
            }
        }

        public async Task<JsonResult> GetSubCategories(int mainCategoryId)
        {
            return Json(await categoryService.GetCategoryChildrenAsync(mainCategoryId));
        }

    }
}
