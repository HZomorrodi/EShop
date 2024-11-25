using AspNetCoreGeneratedDocument;
using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.Services.EFServices;
using EShop.ViewModels.Products;
using EShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace EShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public IProductService _productService { get; }
        public IUnitOfWork _uow { get; }

        public HomeController(ILogger<HomeController> logger, IProductService productService, IUnitOfWork uow)
        {
            _logger = logger;
            _productService = productService;
            _uow = uow;
        }

        public IActionResult Index()
        {
            //Product prouducts = _productService.FindById(7);
            //prouducts.First().Description = "des151";
            //_productService.Remove(2);
            //await _uow.SaveChangesAsync();
            return View();
        }
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel product)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "khit kashti");
            }
            _productService.Add(new Product()
            {
                Description = product.Description,
                Title = product.Title,
                Price = product.Price,
            });
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveAsync(int id)
        {
            _productService.Remove(id);
            await _uow.SaveChangesAsync();
            return View("RemoveAsync");
        }
         
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
