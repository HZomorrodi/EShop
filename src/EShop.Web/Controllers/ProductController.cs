using EShop.Services.Contracts;
using EShop.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShop.Web.Controllers
{
    public class ProductController(IProductService productService) : Controller
    {
        private readonly IProductService _productService = productService;

        public async Task<IActionResult> Index(string searchKey = "")
        {
            ProductCartsWithPagination productsWithPagination = await _productService.GetProductsWithFilterAndPagination(searchKey);
            SearchingProductsViewModel model = new()
            {
                Products = productsWithPagination.Products,
            };
            return View(model);
        }
        //[Route("Product/{Id}/{title}")]
        public async Task<IActionResult> Details(int id, string title)
        {
            if (id < 1)
                return View("NotFound");
            ProductDetailsViewModel? productDetails = await _productService.GetProductDetails(id);
            if (productDetails is null)
                return View("NotFound");
            return View(productDetails);
        }
    }
}
