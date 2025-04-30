using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Entities.Identity
{
    public class Role() : IdentityRole<int>
    {
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}