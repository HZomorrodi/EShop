using EShop.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Categories
{
    public class AddCategoryViewModel
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        public string Title { get; set; }
        [Display(Name = "زیردسته")]
        public int ParentId { get; set; }
    }
}
