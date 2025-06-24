using EShop.Entities;
using EShop.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.Contracts
{
    public interface ICategoryService : IGenericService<Category>
    {
        Task<List<ShowCategory>> AllMainCategoriesAsync();

        Task<List<ShowCategory>> AllMainCategoriesAsync(int currentCategoryId);

        Task<List<ShowCategory>> GetCategoryChildrenAsync(int mainCatId);


        Task<List<CategoryAllFields>> GetAllFieldsAsync();
        Task<List<CategoryAllFields>> GetAllFieldsAsync2();

        Category GetToDelete(int id);
    }
}
