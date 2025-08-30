using EShop.Common;
using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Services.Contracts;
using EShop.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EShop.Web.Pages.AdminPage.Sliders
{
    public class EditModel(ISliderService sliderService, IUnitOfWork uow, IProductService productService) : BasePageModel
    {
        private readonly IProductService _productService = productService;
        private readonly IUnitOfWork _uow = uow;
        private readonly ISliderService _sliderService = sliderService;
        [BindProperty]
        public EditSliderViewModel? Slider { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (Id < 1)
                return RedirectToPage("NotFound");
            Slider = await _sliderService.GetForEdit(Id);   
            if (Slider is null)
                return RedirectToPage("NotFound");
            ViewData["Products"]  = (await _productService.GetProductForComboBox()).CreateSelectListItem(selectedItem: Slider.ProductId);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync() 
        {
            if (!ModelState.IsValid)
            {
                ViewData["Products"] = (await _productService.GetProductForComboBox()).CreateSelectListItem(selectedItem: Slider.ProductId);
                ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
                return Page();
            }

            var slider = await _sliderService.FindByIdAsync(Slider.Id);
            if (slider is null)
                return RedirectToPage("NotFound");
            slider.FirstTitle = Slider.FirstTitle;
            slider.SecondTitle = Slider.SecondTitle;
            slider.ProductId = Slider.ProductId;
            if (Slider.Image?.Length > 0)
            {
                WorkWithImages.RemoveImage(slider.Image, "sliders");
                var imageExtension = Path.GetExtension(Slider.Image.FileName);
                var imageName = Guid.NewGuid().ToString("N");
                Slider.Image.SaveImage(imageName, imageExtension, "sliders");
                slider.Image = imageName + imageExtension;
            }
            _sliderService.Update(slider);
            await _uow.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
