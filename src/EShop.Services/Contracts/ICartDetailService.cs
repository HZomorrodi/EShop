using EShop.Entities;
using EShop.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.Contracts
{
    public interface ICartDetailService : IGenericService<CartDetail>
    {
        Task<CartDetail?> GetCartDetailsBy(int productId, int userId);
        Task<int> CalculateUserCartTotalPriceAsync(int userId);
        Task<List<CartDetailPreviewViewModel>> GetCartDetailsByAsync(int userId);
        Task<List<CartDetailPreviewViewModel>> GetCartDetailsAsync(int userId, int cartId);
        Task<List<CartDetailPreviewForAdminViewModel>> GetCartDetailsForAdminAsync(int cartId);
    }
}
