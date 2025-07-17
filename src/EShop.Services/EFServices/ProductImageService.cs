using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.EFServices
{
    public class ProductImageService(IUnitOfWork uow) : GenericService<ProductImage>(uow), IProductImageService
    {
        private readonly IUnitOfWork uow = uow;
        private readonly DbSet<ProductImage> entity = uow.Set<ProductImage>();

        public async Task<ProductImage?> RemoveProductImageByNameAsync(string productImageName)
        {
           return await entity.Where(p => p.Title == productImageName).SingleOrDefaultAsync();
        }
    }
}
