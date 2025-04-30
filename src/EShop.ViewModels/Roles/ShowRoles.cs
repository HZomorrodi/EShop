using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Roles
{
    public class ShowRoles
    {
        public int Id { get; set; }
        [DisplayName("عنوان")]
        public string Name { get; set; }
        [DisplayName("تعداد کاربران در این نقش")]
        public int UsersCount { get; set; }
    }
}
