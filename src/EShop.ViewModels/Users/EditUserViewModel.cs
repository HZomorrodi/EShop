using EShop.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Users
{
    public class EditUserViewModel
    {
        [HiddenInput]
        public int Id { get; set; } 
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MinLength(3, ErrorMessage = AttributesErrorMessages.MinErrorMessage)]
        [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        [RegularExpression(@"^\w+$", ErrorMessage = "نام کاربری باید از حروف انگلیسی تشکیل شده باشد")]
        [Remote("CheckUserName", "Account", null, ErrorMessage = AttributesErrorMessages.RemoteErrorMessage)]
        public string UserName { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$", ErrorMessage = AttributesErrorMessages.RegularExpressionErrorMessage)]
        [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        public required string  Email { get; set; }
        [Display(Name = "رمز عبور")]
        [MinLength(6, ErrorMessage = AttributesErrorMessages.MinErrorMessage)]
        [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Display(Name = "تکرار رمز عبور")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage =AttributesErrorMessages.CompareErrorMessage)]
        public string? ConfirmPassword { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MinLength(3, ErrorMessage = AttributesErrorMessages.MinErrorMessage)]
        [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$",
        ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MinLength(3, ErrorMessage = AttributesErrorMessages.MinErrorMessage)]
        [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$",
        ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
        public string LastName { get; set; }
        [Display(Name = "نقش های کاربر")]
        public List<string> SelectedRoles { get; set; } = [];
    }
}
