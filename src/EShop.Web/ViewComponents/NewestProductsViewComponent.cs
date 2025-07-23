using EShop.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShop.Web.ViewComponents
{
    public class NewestProductsViewComponent(IProductService productService) : ViewComponent
    {
        private readonly IProductService _productService = productService;

        public async Task<IViewComponentResult> InvokeAsync(int? excludeId)
        {
            return View(await _productService.GetNewestProductAsync(excludeId));
        }
    }
}
