using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Entities
{
    public class ProductImage : BaseEntity
    {
        #region Fields
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        #endregion
        #region Relations
        public virtual Product Product { get; set; }
        #endregion
    }
}