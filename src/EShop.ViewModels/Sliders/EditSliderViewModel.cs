using EShop.Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Sliders
{
    public class EditSliderViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "محصول")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(1, int.MaxValue, ErrorMessage = "لطفا محصول را انتخاب کنید")]
        public int ProductId { get; set; }

        [Display(Name = "عنوان اول")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        public string FirstTitle { get; set; }

        [Display(Name = "عنوان دوم")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        public string SecondTitle { get; set; }

        [Display(Name = "عکس اسلایدر")]
        public IFormFile Image { get; set; }
    }
}
