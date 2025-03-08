using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Entities
{
    public class User : IdentityUser<int>
    {
        public DateTime CreatedDateTime { get; set; }
    }
}
