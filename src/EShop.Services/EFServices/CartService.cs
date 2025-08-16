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
    public class CartService(IUnitOfWork uow) : GenericService<Cart>(uow), ICartService
    {
        private readonly IUnitOfWork uow = uow;
        private readonly DbSet<Cart> _cart = uow.Set<Cart>();

        public async Task<Cart?> GetUserCartAsync(int userId)
        {
            return await _cart.Where(c => c.UserId == userId && !c.IsPay).SingleOrDefaultAsync();
        }

        public async Task<List<ShowCartPreviewForClientViewModel>> GetUserCartsForClient(int userId)
        {
            return await _cart.Where(c => c.UserId == userId).Select(c => new ShowCartPreviewForClientViewModel()
            {
                Id = c.Id,
                IsPay = c.IsPay,
                TotalPrice = c.TotalPrice,
                Address = c.Address,
                RefId = c.RefId,
            }).ToListAsync();
        }

        public async Task<List<ShowCartPreviewForAdminViewModel>> GetUserCartsForAdmin()
        {
            return await _cart.Where(c => c.IsPay).Select(c => new ShowCartPreviewForAdminViewModel()
            {
                Id = c.Id,
                CustomerFullName = c.User.FullName,
                IsPay = c.IsPay,
                TotalPrice = c.TotalPrice,
                RefId = c.RefId,
                Address = c.Address,
            }).ToListAsync();
        }
    }
}
