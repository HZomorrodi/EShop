using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Cart
{
    public class CartDetailPreviewViewModel
    {
        public int ProductId { get; set; }
        [Display(Name = "عنوان محصول")]
        public string ProductTitle { get; set; }
        [Display(Name = "عکس محصول")]
        public string ProductImage { get; set; }
        [Display(Name = "تعداد")]
        public int Count { get; set; }
        [Display(Name = "قیمت واحد")]
        public int Price { get; set; }
        [Display(Name = "مجموع قیمت")]
        public int TotalPrice { get { return Price * Count; } }
    }
}
