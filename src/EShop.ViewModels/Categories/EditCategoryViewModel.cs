using EShop.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Categories
{
    public class EditCategoryViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        public string Title { get; set; }
        [Display(Name = "زیردسته")]
        public int? ParentId { get; set; }
    }
}
