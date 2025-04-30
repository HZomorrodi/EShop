using EShop.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Roles
{
    public class AddRoleViewModel
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        [Remote("CheckRoleAccount", "Role", "Admin",
            ErrorMessage = AttributesErrorMessages.RemoteErrorMessage, HttpMethod = "POST")]
        public string Name { get; set; }
    }
}
