using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Common
{
    public static class CommonExtensionMethods
    {
        public static List<SelectListItem> CreateSelectListItem<T>(this List<T> list, object selectedItem = null,
                                                                   bool addChooseOneItem = true, string firstItemText = "انتخاب کنید",
                                                                   string firstItemValue = "0")
        {
            List<SelectListItem> selectListItem = [];
            if (addChooseOneItem)
            {
                selectListItem.Add(new SelectListItem(firstItemText, firstItemValue));
            }
            if (list.Count != 0)
            {
                Type type = list.First().GetType();
                System.Reflection.PropertyInfo? propertyInfoId = type.GetProperty("Id");
                System.Reflection.PropertyInfo? propertyInfoTitle = type.GetProperty("Title");
                if (propertyInfoId is null || propertyInfoTitle is null)
                {
                    throw new ArgumentNullException($"{typeof(T).Name} Id or Title Property is null");
                }

                foreach (var item in list)
                {
                    string? id = propertyInfoId.GetValue(item)?.ToString();
                    string? title = propertyInfoTitle.GetValue(item)?.ToString();
                    bool selected = selectedItem?.ToString() == id;
                    selectListItem.Add(new SelectListItem(title, id, selected));
                }
            }
            return selectListItem;
        }
    }
}
