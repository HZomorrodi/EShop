using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Sliders
{
    public class ShowSliderViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "محصول")]
        public string ProductTitle { get; set; }

        [Display(Name = "عنوان اول")]
        public string FirstTitle { get; set; }

        [Display(Name = "عنوان دوم")]
        public string SecondTitle { get; set; }
    }
}
