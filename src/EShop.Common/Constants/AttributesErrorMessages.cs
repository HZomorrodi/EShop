using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Common.Constants
{
    public static class AttributesErrorMessages
    {
        public const string RequiredErrorMessage = "لطفا {0} را وارد نمائید";
        public const string MinErrorMessage = "{0}  نباید کمتر از {1} کارکتر باشد";
        public const string MaxErrorMessage = "{0} وارد شده نباید بیشتر از {1} کارکتر باشد";
        public const string StringLengthErrorMessage = "{0}  باید بین {2} کارکتر و {1} کارکتر باشد ";
        public const string RegularExpressionErrorMessage = " {0} را به درستی وارد کنید ";
        public const string RemoteErrorMessage = "  این {0} قبلا درسیستم ثبت شده است";
        public const string CompareErrorMessage = "{1} با تکرار آن تطابق ندارد";
    }
}
