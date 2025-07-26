using EShop.Services.Contracts;
using EShop.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShop.Web.Controllers
{
    public class ProductController(IProductService productService) : Controller
    {
        private readonly IProductService _productService = productService;

        public IActionResult Index()
        {
            return View();
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
