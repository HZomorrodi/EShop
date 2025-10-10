using EShop.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Account.WebApi
{
    /// <summary>
    /// Login model that must get ```UserName``` ```Password``` ```RememberMe```
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// The ```UserName``` of the user
        /// </summary>
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(10, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        public string UserName { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(20, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        public string Password { get; set; }

        /// <summary>
        /// If remember me be true, token lifetime will set with 90 days
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
