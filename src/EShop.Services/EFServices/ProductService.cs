using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.ViewModels.Categories;
using EShop.ViewModels.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.EFServices
{
    public class ProductService(IUnitOfWork uow) : GenericService<Product>(uow), IProductService
    {
        public readonly DbSet<Product> _products = uow.Set<Product>();
        public readonly IUnitOfWork _uow = uow;

        public async Task<EditProductViewModel?> GetProductToEdit(int id)
        {
            return await _products
                .Where(p => p.Id == id)
                .Select(p => new EditProductViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = p.Price,
                    Description = p.Description,
                    CategoryId = p.Category.ParentId ?? 0,
                    CategoryChildrenId = p.CategoryId,
                    ProductImages = p.ProductImages.Select(i => i.Title).ToList(),
                    Properties = p.ProductProperties.Select(p => $"{p.Title} ||| {p.Value}").ToList(),
                }).SingleOrDefaultAsync();
        }

        public async Task<Product?> GetProductToUpdateAsync(int id)
        {
            return await _products
                .Where(p => p.Id == id)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductProperties)
                .SingleOrDefaultAsync();
        }

        public List<ShowProductViewModel> GetProductsPreview()
        {
            return [.. _products.Select(p => new ShowProductViewModel()
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                CategoryTitle = p.Category.Title,
                CanRemove = !p.CartDetails.Any(),
            })];
        }

        public async Task<ProductDetailsViewModel?> GetProductDetails(int productId)
        {
            return await _products
                      .Where(p => p.Id == productId)
                      .Select(p => new ProductDetailsViewModel()
                      {
                          Id = p.Id,
                          Title = p.Title,
                          Price = p.Price,
                          Description = p.Description,
                          CategoryTitle = p.Category.Title,
                          Images = p.ProductImages.Select(i => i.Title).ToList(),
                          Properties = p.ProductProperties.Select(i => $"{i.Title} ||| {i.Value}").ToList(),
                      }).SingleOrDefaultAsync();
        }
        public async Task<List<ProductPreviewViewModel>> GetNewestProductAsync(int? excludeId = null, int take = 8)
        {
            return await _products
                   .OrderByDescending(p => p.Id)
                   .Take(take)
                   .Where(p => excludeId == null || p.Id != excludeId)
                   .Select(p => new ProductPreviewViewModel()
                   {
                       Id = p.Id,
                       Title = p.Title,
                       Image = p.ProductImages.First().Title,

                   }).ToListAsync();
        }
        public async Task<List<ProductPreviewViewModel>> GetBestSellingProductAsync(int take = 5)
        {
            return await _products.Select(p => new ProductPreviewViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Image = p.ProductImages.OrderBy(img => img.Id).Select(img => img.Title).FirstOrDefault(),
                SoldCount = p.CartDetails.Where(cd => cd.Cart.IsPay).Sum(cd => (int?)cd.Count) ?? 0,
            })
            .Where(x => x.SoldCount > 0).
            OrderByDescending(x => x.SoldCount).
            Take(take).
            ToListAsync();
        }

        public int GetMinPrice()
        {
            return _products.Min(x => x.Price);
        }
        public int GetMaxPrice()
        {
            return _products.Max(x => x.Price);
        }
        public async Task<Product?> GetProductToDelete(int id)
        {
            return await _products
                .Where(p => p.Id == id && !p.CartDetails.Any())
                .Include(p => p.ProductImages)
                .SingleOrDefaultAsync();
        }

        public Task<List<ShowProductInComboBoxViewModel>> GetProductForComboBox()
        => _products.Select(x => new ShowProductInComboBoxViewModel()
        {
            Id = x.Id,
            Title = x.Title
        }).ToListAsync();
    }
}
