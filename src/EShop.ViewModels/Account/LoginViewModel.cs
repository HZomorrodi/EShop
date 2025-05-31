using EShop.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Account
{
    public class LoginViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MinLength(3, ErrorMessage = AttributesErrorMessages.MinErrorMessage)]
        [MaxLength(30, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
        public List<Microsoft.AspNetCore.Authentication.AuthenticationScheme> ExternalLogins { get; set; }
    }
}
