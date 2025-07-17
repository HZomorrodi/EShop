using EShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.Contracts
{
    public interface IProductImageService : IGenericService<ProductImage>
    {
        Task<ProductImage?> RemoveProductImageByNameAsync(string productImageName);
    }
}
