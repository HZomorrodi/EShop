using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Common.Attributes
{
    public class MaxFileSizeAttribute : BaseValidationAttribute, IClientModelValidator
    {
        private readonly int _maxFileSize;
        private readonly string _errorMessage;
        public MaxFileSizeAttribute(string displayName, int maxFileSize)
        {
            _maxFileSize = maxFileSize * 1024 * 1024;
            _errorMessage = $"اندازه {displayName} نباید بیشتر از {maxFileSize} مگابایت باشد";
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > 0)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(_errorMessage);
                }
            }
            return ValidationResult.Success;
        }
        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-maxFileSize", _errorMessage);
            MergeAttribute(context.Attributes, "data-val-maxsize", _maxFileSize.ToString());
        }
    }
}
