using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Entities
{
    public class Product : BaseEntity
    {
        #region Fields
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        #endregion
        #region Relations

        public virtual Category Category { get; set; } 
        public virtual ICollection<CartDetail> CartDetails { get; set; } = [];
        public virtual ICollection<ProductImage> ProductImages { get; set; } = [];
        public virtual ICollection<ProductProperty> ProductProperties { get; set; } = [];
        public virtual ICollection<ProductProductTag> ProductProductTags { get; set; } = [];
        #endregion

    }
}

