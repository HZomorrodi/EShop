using EShop.Entities;
using EShop.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.Contracts
{
    public interface ICartService : IGenericService<Cart>
    {
        Task<Cart?> GetUserCartAsync(int userId);
        Task<List<ShowCartPreviewForClientViewModel>> GetUserCartsForClient(int userId);
        Task<List<ShowCartPreviewForAdminViewModel>> GetUserCartsForAdmin();
    }
}
