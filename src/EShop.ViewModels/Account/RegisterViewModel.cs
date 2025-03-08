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
    public class RegisterViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MinLength(4, ErrorMessage = AttributesErrorMessages.MinErrorMessage)]
        [MaxLength(40, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        [Remote("CheckUserAccount", "Account", null,
            ErrorMessage = AttributesErrorMessages.RemoteErrorMessage, HttpMethod = "POST")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [EmailAddress(ErrorMessage = AttributesErrorMessages.RegularExpressionErrorMessage)]
        public string Email { get; set; }
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MinLength(4, ErrorMessage = AttributesErrorMessages.MinErrorMessage)]
        [MaxLength(40, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = AttributesErrorMessages.CompareErrorMessage)]
        public string ConfirmPassword { get; set; }

    }
}
