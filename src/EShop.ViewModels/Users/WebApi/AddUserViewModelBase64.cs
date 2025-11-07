using EShop.Common.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Users.WebApi
{
    public class AddUserViewModelBase64
    {
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)] 
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }

        public List<string>? Roles { get; set; }

        public string Avatar { get; set; }
    }
}
