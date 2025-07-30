using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Cart
{
    public class ShowCartPreviewForAdminViewModel
    {
        public int Id { get; set; }
        public string CustomerFullName { get; set; }
        public int TotalPrice { get; set; }
        public bool IsPay { get; set; }
        public int RefId { get; set; }
        public string Address { get; set; }

    }
}
