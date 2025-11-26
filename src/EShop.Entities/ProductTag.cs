using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Entities
{
    [Index(nameof(Title), IsUnique = true)]
    public class ProductTag : BaseEntity
    {
        #region Fields
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        #endregion

        #region Relations
        public ICollection<ProductProductTag> ProductProductTags { get; set; } = [];
        #endregion


    }
}
