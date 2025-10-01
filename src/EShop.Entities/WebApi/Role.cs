using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Entities.WebApi
{
    [Index(nameof(Title), IsUnique = true)]
    public class Role : BaseEntity
    {
        #region Fields
        public string Title { get; set; }
        #endregion
        #region Relations
        public virtual ICollection<User> Users { get; set; } = [];
        #endregion
    }
}
