using EShop.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Entities
{
    public class CartDetail : BaseEntity
    {
        #region Fields
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        #endregion  
        #region Relations
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
        #endregion
    }
}
