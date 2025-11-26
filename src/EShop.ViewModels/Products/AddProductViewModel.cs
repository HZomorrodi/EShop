using EShop.Common.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Products
{
    public class AddProductViewModel
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string Description { get; set; }

        [Display(Name = "قیمت")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public int Price { get; set; }

        [Display(Name = "دسته بندی")]
        public int CategoryId { get; set; }

        [Display(Name = "زیر دسته")]
        [Range(1, int.MaxValue, ErrorMessage = "لطفا دسته بندی را انتخاب نمایید")]
        public int CategoryChildrenId { get; set; }

        [Display(Name = "عکس های محصول")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public List<IFormFile> Images { get; set; }

        [Display(Name = "ویژگی های محصول")]
        public List<string> Properties { get; set; } = [];

        [Display(Name = "کلمات کلیدی")]
        public string Tags { get; set; }

    }
}
