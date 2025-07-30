using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Cart
{
    public class CartDetailPreviewViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductImage { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public int TotalPrice { get { return Price * Count; } }
    }
}
