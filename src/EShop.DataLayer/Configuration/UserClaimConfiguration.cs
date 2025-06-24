using EShop.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.DataLayer.Configuration
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("UserClaim").
                HasOne(userClaim => userClaim.User).
                WithMany(user => user.UserClaims).
                HasForeignKey(claim => claim.UserId);
        }
    }
}
