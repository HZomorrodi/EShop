using EShop.Entities;
using EShop.Entities.WebApi;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.DataLayer.Context
{
    public class TicketDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
    {
        public DbSet<Test> Tests { get; set; }

        public void MarkAsDeleted<TEntity>(TEntity entity)
            => base.Entry(entity).State = EntityState.Deleted;
    }
}
