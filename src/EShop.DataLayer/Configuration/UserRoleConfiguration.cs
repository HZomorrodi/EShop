using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.DataLayer.Configuration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole").
                HasOne(userRole => userRole.User).
                WithMany(user => user.UserRole).
                HasForeignKey(userRole => userRole.UserId);

            builder.HasOne(userRole => userRole.Role).
                WithMany(role => role.UserRole).
                HasForeignKey(userRole => userRole.RoleId);
        }
    }
}
