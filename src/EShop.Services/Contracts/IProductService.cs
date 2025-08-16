using EShop.Entities;
using EShop.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.Contracts
{
    public interface IProductService : IGenericService<Product>
    {
        Task<EditProductViewModel?> GetProductToEdit(int id);
        Task<Product?> GetProductToUpdateAsync(int id);
        List<ShowProductViewModel> GetProductsPreview();
        Task<ProductDetailsViewModel?> GetProductDetails(int productId);
        Task<List<ProductPreviewViewModel>> GetNewestProductAsync(int? excludeId = null, int take = 8);
        Task<List<ProductPreviewViewModel>> GetBestSellingProductAsync(int take = 5);
        Task<List<ShowProductInComboBoxViewModel>> GetProductForComboBox();
        int GetMinPrice();
        int GetMaxPrice();
        Task<Product?> GetProductToDelete(int id);
    }
}
