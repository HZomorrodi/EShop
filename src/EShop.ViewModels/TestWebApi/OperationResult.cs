using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.TestWebApi
{
    public class OperationResult<TResult>(bool isSuccess, TResult result)
    {
        public bool IsSuccess { get; set; } = isSuccess;

        public TResult Result { get; set; } = result;
    }
}
