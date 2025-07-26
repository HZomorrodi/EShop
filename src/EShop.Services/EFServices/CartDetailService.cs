using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Entities.Identity;
using EShop.Services.Contracts;
using EShop.ViewModels.Cart;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.EFServices
{
    public class CartDetailService(IUnitOfWork uow) : GenericService<CartDetail>(uow), ICartDetailService
    {
        private readonly IUnitOfWork uow = uow;
        private readonly DbSet<CartDetail> _cartDetail = uow.Set<CartDetail>();

        public async Task<CartDetail?> GetCartDetailsBy(int productId, int userId)
        {
            return await _cartDetail.Where(c => c.Cart.UserId == userId &&
            c.ProductId == productId && !c.Cart.IsPay).SingleOrDefaultAsync();
        }

        public async Task<int> CalculateUserCartTotalPriceAsync(int userId)
        {
            return await _cartDetail.Where(c => c.Cart.UserId == userId
            && !c.Cart.IsPay).SumAsync(c => c.Count * c.Price);
        }

        public async Task<List<CartDetailPreviewViewModel>> GetCartDetailsByAsync(int userId)
        {
            return await _cartDetail.Where(c => c.Cart.UserId == userId &&
            !c.Cart.IsPay).Select(c => new CartDetailPreviewViewModel()
            {
                ProductId = c.ProductId,
                Count = c.Count,
                Price = c.Price,
                ProductImage = c.Product.ProductImages.First().Title,
                ProductTitle = c.Product.Title,
            }).ToListAsync();
        }

        public async Task<List<CartDetailPreviewViewModel>> GetCartDetailsAsync(int userId, int cartId)
        {
            return await _cartDetail.Where(c => c.Cart.UserId == userId &&
            c.CartId == cartId).Select(c => new CartDetailPreviewViewModel()
            {
                ProductId = c.ProductId,
                Count = c.Count,
                Price = c.Price,
                ProductImage = c.Product.ProductImages.First().Title,
                ProductTitle = c.Product.Title,
            }).ToListAsync();
        }

        public async Task<List<CartDetailPreviewForAdminViewModel>> GetCartDetailsForAdminAsync(int cartId)
        {
            return await _cartDetail.Where(c => c.CartId == cartId && c.Cart.IsPay).
                Select(c => new CartDetailPreviewForAdminViewModel()
                {
                    ProductId = c.ProductId,
                    CustomerFullName = c.Cart.User.FullName,
                    Count = c.Count,
                    Price = c.Price,
                    ProductImage = c.Product.ProductImages.First().Title,
                    ProductTitle = c.Product.Title,
                }).ToListAsync();
        }
    }
}
