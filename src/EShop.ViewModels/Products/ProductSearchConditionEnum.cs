using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Products
{
    public enum ProductSearchConditionEnum
    {
        [Display(Name = "پر فروشترین")]
        BestSelling,
        [Display(Name = "جدید ترین")]
        Newest,
        [Display(Name = "قدیمی ترین")]
        Oldest,
        [Display(Name = "ارزانترین")]
        Cheapest,
        [Display(Name = "گرانترین")]
        MostExpensive,
    }
}
