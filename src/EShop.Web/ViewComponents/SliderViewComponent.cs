using EShop.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShop.Web.ViewComponents
{
    public class SliderViewComponent(ISliderService sliderService) : ViewComponent
    {
        private readonly ISliderService _sliderService = sliderService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _sliderService.GetSlidersForFront());
        }
    }
}
