using EShop.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShop.Web.ViewComponents
{
    public class BestSellingProductsViewComponent(IProductService productService) : ViewComponent
    {
        private readonly IProductService productService = productService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ViewModels.Products.ProductPreviewViewModel> model = await productService.GetBestSellingProductAsync();
            return View(model);
        }
    }
}
