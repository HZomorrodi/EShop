using EShop.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Entities
{
    public class Cart : BaseEntity
    {
        #region Fields
        public int UserId { get; set; }
        public int TotalPrice { get; set; }
        public bool IsPay { get; set; }
        public int RefId { get; set; }
        [MaxLength(300)]
        public string Address { get; set; }
        #endregion
        #region Relations
        public virtual User User { get; set; }
        public virtual ICollection<CartDetail> CartDetails { get; set; } = [];
        #endregion
    }
}
