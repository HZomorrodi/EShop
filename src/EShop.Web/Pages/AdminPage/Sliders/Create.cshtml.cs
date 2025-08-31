using EShop.Common;
using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Entities.Identity;
using EShop.Services.Contracts;
using EShop.Services.EFServices;
using EShop.ViewModels.Sliders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EShop.Web.Pages.AdminPage.Sliders
{
    public class CreateModel(ISliderService sliderService, IUnitOfWork uow, IProductService productService) : BasePageModel
    {
        private readonly IProductService _productService = productService;
        private readonly IUnitOfWork _uow = uow;
        private readonly ISliderService _sliderService = sliderService;
        public AddSliderViewModel? Slider { get; set; }
        public async Task OnGetAsync()
        {
            ViewData["Products"] = (await _productService.GetProductForComboBox()).CreateSelectListItem();
        }

        public async Task<IActionResult> OnPostAsync(AddSliderViewModel slider)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Products"] = (await _productService.GetProductForComboBox()).CreateSelectListItem(selectedItem: slider.ProductId);
                ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
                return Page();
            }
            var imageExtension = Path.GetExtension(slider.Image.FileName);
            var imageName = Guid.NewGuid().ToString("N");
            slider.Image.SaveImage(imageName, imageExtension, "sliders");
            await _sliderService.AddAsync(new Slider()
            {
                FirstTitle = slider.FirstTitle,
                SecondTitle = slider.SecondTitle,
                ProductId = slider.ProductId,
                Image = imageName + imageExtension
            });
            await _uow.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
