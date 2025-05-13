using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Users
{
    public class ShowUsersViewModel
    {
        public int Id { get; set; }
        [DisplayName("نام کاربری")]
        public string UserName { get; set; }
        [DisplayName("نام و نام خانوادگی")]
        public string FullName { get; set; }
        [DisplayName("تاریخ عضویت")]
        public DateTime CreatedDateTime { get; set; }
        public bool IsActive { get; set; }
    }
}
