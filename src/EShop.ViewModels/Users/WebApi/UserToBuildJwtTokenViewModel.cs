using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Users.WebApi
{
    public class UserToBuildJwtTokenViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
