using EShop.Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.TestWebApi
{
    public class AddUserViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        public string UserName { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        public string FullName { get; set; }
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        public string Password { get; set; }

        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public IFormFile UserAvatar { get; set; }
        public string? Avatar { get; set; }
    }
}
