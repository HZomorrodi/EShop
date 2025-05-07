using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Common.Constants
{
    public static class DateTimeExtensionMethod
    {
        public static string DateTimeToPersian(this DateTime dt)
        {
            PersianCalendar persianCalendar = new();
            return $"{persianCalendar.GetYear(dt)}/{persianCalendar.GetMonth(dt)}/{persianCalendar.GetDayOfMonth(dt)}";
        }
    }
}
