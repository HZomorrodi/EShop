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
    public class ProductService(IUnitOfWork uow) : GenericService<Product>(uow), IProductService
    {
        public readonly DbSet<Product> _product = uow.Set<Product>();
        public readonly IUnitOfWork _uow = uow;

    }
}
