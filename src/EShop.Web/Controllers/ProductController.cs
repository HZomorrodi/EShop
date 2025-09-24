using EShop.Services.Contracts;
using EShop.Services.EFServices;
using EShop.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EShop.Web.Controllers
{
    public class ProductController(IProductService productService, ICategoryService categoryService) : Controller
    {
        private readonly IProductService _productService = productService;
        private readonly ICategoryService categoryService = categoryService;

        public async Task<IActionResult> Index(SearchingProductsViewModel model,string s,
    List<int> selectedCategories,
    ProductSearchConditionEnum condition = ProductSearchConditionEnum.Newest,
    int page = 1)
        {
            ProductCartsWithPagination productsWithPagination = await _productService.GetProductsWithFilterAndPagination(model);
            model.Products = productsWithPagination.Products;
            model.Page = productsWithPagination.Page;
            model.AllPagesCount = productsWithPagination.AllPagesCount;
            model.MinPrice = _productService.GetMinPrice();
            model.MaxPrice = _productService.GetMaxPrice();
            ViewBag.Categories = await categoryService.GetAllFieldsAsync2();
            ViewBag.searchKey = "کفش";
                return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] ProductSearchViewModel searchModel)
        {
            // Start with all products
            IQueryable<Entities.Product> query = _productService.GetProductQuery();

            // Apply filters based on query parameters
            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                query = query.Where(p => p.Title.Contains(searchModel.Name));
            }

            if (!string.IsNullOrEmpty(searchModel.Category))
            {
               // query = query.Where(p => p.Category == searchModel.Category);
            }

            if (searchModel.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= searchModel.MinPrice.Value);
            }

            if (searchModel.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= searchModel.MaxPrice.Value);
            }

            if (searchModel.InStockOnly)
            {
                //query = query.Where(p => p.StockQuantity > 0);
            }

            // Apply sorting
            switch (searchModel.SortBy)
            {
                case "price_asc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                case "name":
                    query = query.OrderBy(p => p.Title);
                    break;
                default:
                    query = query.OrderBy(p => p.Id);
                    break;
            }

            var products = await query.ToListAsync();

            // Return as JSON for AJAX or view for full page
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ProductList", products);
            }

            return View(products);
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
