using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.Services.EFServices;
using EShop.ViewModels.Products;
using EShop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EShop.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IProductService productService, ICategoryService categoryService, IUnitOfWork uow) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        public IProductService _productService { get; } = productService;
        private readonly ICategoryService categoryService = categoryService;
        public IUnitOfWork _uow { get; } = uow;

        public async Task<IActionResult> Index()
        {
            return View(await categoryService.GetAllFieldsAsync2());
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
