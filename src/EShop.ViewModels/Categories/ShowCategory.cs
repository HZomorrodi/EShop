using EShop.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Categories
{
    public class ShowCategory
    {
        public int Id { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        public bool CanRemove { get; set; }
        public List<ShowCategory> Children { get; set; }

    }
}
