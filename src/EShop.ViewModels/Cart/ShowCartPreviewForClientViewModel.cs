using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Cart
{
    public class ShowCartPreviewForClientViewModel
    {
        public int Id { get; set; }
        [Display(Name = "مجموع قیمت")]
        public int TotalPrice { get; set; }
        [Display(Name = "وضعیت پرداخت")]
        public bool IsPay { get; set; }
        [Display(Name = "شماره پیگیری")]
        public string? RefId { get; set; }
        [Display(Name = "آدرس")]
        public string? Address { get; set; }

    }
}
