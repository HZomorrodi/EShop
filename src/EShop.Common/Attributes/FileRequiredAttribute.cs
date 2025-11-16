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
    public class FileRequiredAttribute : BaseValidationAttribute, IClientModelValidator
    {
        private readonly string _errorMessage;

        public FileRequiredAttribute(string displayName)
        {
            _errorMessage = $"لطفا {displayName} را وارد نمایید";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //var displayName = validationContext.DisplayName;
            if (value is not IFormFile file || file.Length == 0)
            {
                return new ValidationResult(_errorMessage);
            }
            return ValidationResult.Success;
        }
        public void AddValidation(ClientModelValidationContext context)
        {
            //var displayName = context.ModelMetadata.DisplayName;
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-fileRequired", _errorMessage);
        }

    }
}
