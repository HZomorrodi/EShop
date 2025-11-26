using EShop.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.DataLayer.Configuration
{
    public class ProductProductTagConfiguration : IEntityTypeConfiguration<ProductProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductProductTag> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.ProductTagId });
            builder.HasOne(x => x.Product)
       .WithMany(p => p.ProductProductTags)
       .HasForeignKey(x => x.ProductId);
            builder.HasOne(x => x.ProductTag)
       .WithMany(p => p.ProductProductTags)
       .HasForeignKey(x => x.ProductTagId);
            builder.ToTable("ProductsProductTags");
        }
    }
}
