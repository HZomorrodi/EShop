using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BaseValidationAttribute : ValidationAttribute
    {
        protected static void MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            attributes.TryAdd(key, value);
        }
    }
}
