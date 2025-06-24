using EShop.Entities;
using EShop.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.DataLayer.Context
{
    public class EShopDbContext(DbContextOptions options) : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options),
        IUnitOfWork
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(EShopDbContext).Assembly);
        }
        public void MarkAsDeleted<TEntity>(TEntity entity)
    => base.Entry(entity).State = EntityState.Deleted;
    }

    

}
