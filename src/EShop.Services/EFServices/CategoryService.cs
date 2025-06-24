using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.ViewModels.Categories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.EFServices
{
    public class CategoryService(IUnitOfWork uow) : GenericService<Category>(uow), ICategoryService
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly DbSet<Category> _categories = uow.Set<Category>();

        public Task<List<ShowCategory>> AllMainCategoriesAsync()
        => _categories
            .Where(category => category.ParentId == null)
            .Select(category => new ShowCategory()
            {
                Id = category.Id,
                Title = category.Title,
                CanRemove = !category.Children.Any()
            }).ToListAsync();

        public Task<List<ShowCategory>> AllMainCategoriesAsync(int currentCategoryId)
       => _categories
           .Where(category => category.ParentId == null)
           .Where(category => category.Id != currentCategoryId)
           .Select(category => new ShowCategory()
           {
               Id = category.Id,
               Title = category.Title,
               CanRemove = category.Children.Any()
           }).ToListAsync();
        public Task<List<ShowCategory>> GetCategoryChildrenAsync(int mainCatId)
         => _categories
            .Where(category => category.ParentId == mainCatId)
            .Select(category => new ShowCategory()
            {
                Id = category.Id,
                Title = category.Title,
                CanRemove = !category.Products.Any()
            }).ToListAsync();


        public Task<List<CategoryAllFields>> GetAllFieldsAsync()
        => _categories.Select(x => new CategoryAllFields()
        {
            Children = x.Children.Select(c => new CategoryAllFields()
            {
                Id = c.Id,
                Title = c.Title
            }).ToList(),
            Id = x.Id,
            ParentId = x.ParentId,
            Title = x.Title
        }).ToListAsync();
        public Task<List<CategoryAllFields>> GetAllFieldsAsync2()
        => _categories.Where(x => x.ParentId == null).Select(x => new CategoryAllFields()
        {
            Children = x.Children.Select(c => new CategoryAllFields()
            {
                Id = c.Id,
                Title = c.Title,
                ParentId = c.ParentId,
            }).ToList(),
            Id = x.Id,
            Title = x.Title
        }).ToListAsync();

        public Category GetToDelete(int id)
            => _categories.Where(x => !x.Children.Any())
                .SingleOrDefault(x => x.Id == id);
    }
}
