using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Entities
{
    public class ProductProperty : BaseEntity
    {
        #region Fields
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        public string Value { get; set; }
        public int ProductId { get; set; }  
        #endregion
        #region Relations
        public virtual Product Product { get; set; }
        #endregion
    }
}
