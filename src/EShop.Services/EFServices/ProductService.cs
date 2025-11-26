using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.ViewModels.Categories;
using EShop.ViewModels.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.EFServices
{
    public class ProductService(IUnitOfWork uow) : GenericService<Product>(uow), IProductService
    {
        public readonly DbSet<Product> _products = uow.Set<Product>();
        public readonly IUnitOfWork _uow = uow;

        public async Task<ProductCartsWithPagination> GetProductsWithFilterAndPagination(SearchingProductsViewModel model)
        {
            IQueryable<Product> product = _products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(model.s))
                product = product.Where(p => p.Title.Contains(model.s.Trim()));
            product = product.Where(p => p.Price >= model.SelectedMinPrice);
            if (model.SelectedMaxPrice > 0)
                product = product.Where(p => p.Price <= model.SelectedMaxPrice);
            if (model.selectedCategories.Count != 0)
                product = product.Where(p => model.selectedCategories.Contains(p.CategoryId));
            product = model.Condition switch
            {
                ProductSearchConditionEnum.BestSelling => product.Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.ProductImages,
                    p.Price,
                    SoldCount = p.CartDetails.Where(cd => cd.Cart.IsPay).Sum(cd => (int?)cd.Count) ?? 0,
                })
            .Where(x => x.SoldCount > 0).
            OrderByDescending(x => x.SoldCount).Select(p => new Product()
            {
                Id = p.Id,
                ProductImages = p.ProductImages,
                Title = p.Title,
                Price = p.Price,
            }),
                ProductSearchConditionEnum.Newest => product.OrderByDescending(p => p.Id),
                ProductSearchConditionEnum.Oldest => product.OrderBy(p => p.Id),
                ProductSearchConditionEnum.Cheapest => product.OrderBy(p => p.Price),
                ProductSearchConditionEnum.MostExpensive => product.OrderByDescending(p => p.Price),
                _ => throw new NotImplementedException(),
            };
            int allRecordsCount = product.Count();
            int allPagesCount = (int)
                (Math.Ceiling(
                    (decimal)allRecordsCount / model.Take
                ));
            if (model.Page < 1)
                model.Page = 1;
            if (model.Page > allPagesCount)
                model.Page = allPagesCount;
            int skip = allPagesCount > 0 ? (model.Page - 1) * model.Take : 0;
            return new ProductCartsWithPagination()
            {
                Products = await product.Skip(skip).Take(model.Take).Select(p => new ProductCartViewModel()
                {
                    Id = p.Id,
                    Image = p.ProductImages.First().Title,
                    Title = p.Title,
                    Price = p.Price,
                }).ToListAsync(),
                AllPagesCount = allPagesCount,
                Page = model.Page,
            };
        }

        public IQueryable<Product> GetProductQuery()
        {
            return _products.AsQueryable();

        }
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
                    SelectedTags = string.Join(",", p.ProductProductTags.Select(x => x.ProductTag.Title))
                }).SingleOrDefaultAsync();
        }

        public async Task<Product?> GetProductToUpdateAsync(int id)
        {
            return await _products.Include(p => p.ProductProductTags)
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
                          Tags = p.ProductProductTags.Select(p => p.ProductTag.Title).ToList(),
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
