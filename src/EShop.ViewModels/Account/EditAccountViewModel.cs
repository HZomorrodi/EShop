using EShop.Common;
using EShop.Common.Attributes;
using EShop.Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Account
{
    public class EditAccountViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MinLength(3, ErrorMessage = AttributesErrorMessages.MinErrorMessage)]
        [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        [RegularExpression(@"^\w+$", ErrorMessage = "نام کاربری باید از حروف انگلیسی تشکیل شده باشد")]
        [Remote("CheckUserName", "Account", null,
            ErrorMessage = AttributesErrorMessages.RemoteErrorMessage, HttpMethod = "POST",
            AdditionalFields = $"{ViewModelConstants.AntiForgeryToken},{nameof(Id)}")]
        public string? UserName { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$", ErrorMessage = AttributesErrorMessages.RegularExpressionErrorMessage)]
        [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        [Remote("CheckEmail", "Account", null,
            ErrorMessage = AttributesErrorMessages.RemoteErrorMessage, HttpMethod = "POST",
            AdditionalFields = ViewModelConstants.AntiForgeryToken)]
        public required string Email { get; set; }
        [Display(Name = "رمز عبور")]
        [MinLength(6, ErrorMessage = AttributesErrorMessages.MinErrorMessage)]
        [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Display(Name = "تکرار رمز عبور")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = AttributesErrorMessages.CompareErrorMessage)]
        public string? ConfirmPassword { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MinLength(3, ErrorMessage = AttributesErrorMessages.MinErrorMessage)]
        [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$",
        ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
        public string? FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MinLength(3, ErrorMessage = AttributesErrorMessages.MinErrorMessage)]
        [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$",
        ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
        public string? LastName { get; set; }
        [DisplayName("آواتار")]
        [FileRequired("آواتار")]
        //[AllowExtensions("آواتار", ["png", "jpg"], ["image/jpeg", "image/png"])]
        [IsImage("آواتار")]
        [MaxFileSize("آواتار", 2)]
        public IFormFile? Avatar { get; set; }
    }
}

