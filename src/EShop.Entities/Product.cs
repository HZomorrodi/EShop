using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Entities
{
    public class Product : BaseEntity
    {
        #region Fields
        [MaxLength(10)]
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        #endregion
        #region Relations

        public virtual Category Category { get; set; }
        #endregion

    }
}

