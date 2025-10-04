using EShop.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Users.WebApi
{
    public class AddUserViewModelBase64
    {
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(100,ErrorMessage = AttributesErrorMessages.MaxErrorMessage)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(100)]
        public string PassWord { get; set; }
        [MaxLength(50)]
        public string Avatar { get; set; }
        public List<string> Roles { get; set; }
    }
}
