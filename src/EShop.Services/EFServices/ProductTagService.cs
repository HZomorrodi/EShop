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
    public class ProductTagService(IUnitOfWork uow) : GenericService<ProductTag>(uow), IProductTagService
    {
        private readonly DbSet<ProductTag> _productTags = uow.Set<ProductTag>();

        public List<ProductTag> GetTags(List<string> splittedTags)
        {
            return _productTags.Where(pt => splittedTags.Contains(pt.Title)).ToList();
        }
    }
}
