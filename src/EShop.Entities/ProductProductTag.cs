using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Entities
{
    public class ProductProductTag  
    {
        #region Fields
        public int ProductId { get; set; }
        public int ProductTagId { get; set; }
        #endregion

        #region Relations
        public Product Product { get; set; }
        public ProductTag ProductTag { get; set; }
        #endregion

    }
}
