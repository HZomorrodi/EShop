using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Entities
{
    public class Slider : BaseEntity
    {
        #region Fields

        [Required]
        [MaxLength(50)]
        public string Image { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstTitle { get; set; }

        [Required]
        [MaxLength(50)]
        public string SecondTitle { get; set; }

        public int ProductId { get; set; }

        #endregion

        #region Relations

        public virtual Product Product { get; set; }

        #endregion
    }
}
