using EShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.Contracts
{
    public interface IProductTagService : IGenericService<ProductTag>
    {
        List<ProductTag> GetTags(List<string> splittedTags);
    }
}
