using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Application
{
    public class StripeConfigsModel
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
}
