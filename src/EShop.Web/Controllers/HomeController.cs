using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.Services.Contracts.Identity;
using EShop.Services.EFServices;
using EShop.ViewModels.Products;
using EShop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EShop.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IProductService productService, ICategoryService categoryService, IUserManagerService userManager, IUnitOfWork uow) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        public IProductService _productService { get; } = productService;
        private readonly ICategoryService categoryService = categoryService;
        private readonly IUserManagerService _userManager = userManager;
        public IUnitOfWork _uow { get; } = uow;

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Hellow, this is the index!");
            _logger.LogWarning("Hellow, this is the index!");
            Entities.Identity.User? user = await _userManager.FindByIdAsync(1.ToString());
            user.UserInformation = new UserInformation()
            {
                BirthDate = DateTime.Now,
                EyeColor = EyeColor.Green,
                FullName = "Tea",
                NationalCode = "1",
            };
            //await _uow.SaveChangesAsync();
            return View(await categoryService.GetAllFieldsAsync2());
        }
        public async Task<IActionResult> RemoveAsync()
        {
            return View("Remove");
        }

        public IActionResult Privacy()
        {
            throw new Exception("IT is Privacy");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
