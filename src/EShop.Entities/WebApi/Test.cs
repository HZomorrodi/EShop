using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Entities.WebApi
{
    public class Test : BaseEntity
    {
        [Required]
        public int MyProperty2 { get; set; }
    }
}
