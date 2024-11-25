using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Products
{
    public class AddProductViewModel
    {
        [Required(ErrorMessage ="{0} را وارد نکردید")]
        public string Title { get; set; }
        [Required(ErrorMessage ="{0} را وارد نکردید")]
        public string Description { get; set; }
        [Required(ErrorMessage ="{0} را وارد نکردید")]
        public int Price { get; set; }
    }
}
