using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Services.Contracts;
using EShop.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EShop.Web.Pages.AdminPage.Sliders
{
    public class IndexModel(ISliderService sliderService, IUnitOfWork uow) : BasePageModel
    {
        private readonly ISliderService _sliderService = sliderService;
        private readonly IUnitOfWork _uow = uow;
        public List<ShowSliderViewModel>? Sliders { get; private set; }
        public async Task OnGetAsync()
        {
            Sliders =  await _sliderService.GetPreviewAsync();
        }
        public async Task<RedirectToPageResult> OnPostDeleteAsync(int id)
        {
            if (id < 1)
                return RedirectToPage("NotFound");
            var slider = await _sliderService.FindByIdAsync(id);
            if (slider is null)
                return RedirectToPage("NotFound");
            _sliderService.Remove(slider);
            await _uow.SaveChangesAsync();
            WorkWithImages.RemoveImage(slider.Image, "sliders");
            return RedirectToPage(nameof(Index));
        }
    }
}
