using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Cart
{
    public class ShowCartPreviewForClientViewModel
    {
        public int Id { get; set; }
        public int TotalPrice { get; set; }
        public bool IsPay { get; set; }
    }
}
