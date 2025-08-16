using EShop.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Cart
{
    public class CheckoutViewModel
    {
        [HiddenInput]
        [Range(1, int.MaxValue, ErrorMessage = AttributesErrorMessages.RangeMessage)]
        public int UserCartTotalPrice { get; set; }
        [DisplayName("آدرس")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(300, ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        public string Address { get; set; }
    }
}
