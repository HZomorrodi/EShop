using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Cart
{
    public class CheckoutViewModel
    {
        public int UserCartTotalPrice { get; set; }
        public string Address { get; set; }
    }
}
