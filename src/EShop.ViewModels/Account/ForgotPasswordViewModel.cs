using EShop.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
       ErrorMessage = AttributesErrorMessages.RegularExpressionErrorMessage)]
        [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        public string Email { get; set; }
    }
}
