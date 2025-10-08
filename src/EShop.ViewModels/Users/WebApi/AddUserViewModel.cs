using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Users.WebApi
{
    public class AddUserViewModel
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }

        public List<string>? Roles { get; set; }

        public IFormFile Avatar { get; set; }
    }
}
