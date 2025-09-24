using EShop.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Products
{
    public class SearchingProductsViewModel
    {
        public string s { get; set; } = "";
        public List<ProductCartViewModel> Products { get; set; }
        public ProductSearchConditionEnum Condition { get; set; } = ProductSearchConditionEnum.Newest;
        public List<int> selectedCategories { get; set; } = [];
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int SelectedMinPrice { get; set; }
        public int SelectedMaxPrice { get; set; }
        public int Page { get; set; } = 1;
        public int AllPagesCount { get; set; }
        public int Take { get; set; } = 2;
    }
}
