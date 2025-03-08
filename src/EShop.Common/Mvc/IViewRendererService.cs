using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Common.Mvc
{
    public interface IViewRendererService
    {
        Task<string> RenderViewToStringAsync(string viewNameOrPath);
        Task<string> RenderViewToStringAsync<TModel>(string viewNameOrPath, TModel model);
    }
}
