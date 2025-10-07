using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Entities.WebApi
{
    [Index(nameof(UserName), IsUnique = true)]
    public class User : BaseEntity
    {
        #region Fields
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(100)]
        public string PassWord { get; set; }
        [MaxLength(50)]
        public string? Avatar { get; set; }
        #endregion
        #region Relations
        public virtual ICollection<Role> Roles { get; set; } = [];
        #endregion
    }
}
