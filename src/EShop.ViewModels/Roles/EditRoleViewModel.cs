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
    public class EditRoleViewModel
    {
        public int Id { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        [Remote("CheckRoleAccountForEdit", "Role", "Admin",
              AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id),
              ErrorMessage = AttributesErrorMessages.RemoteErrorMessage, HttpMethod = "POST")]
        public string Name { get; set; }
    }
}
