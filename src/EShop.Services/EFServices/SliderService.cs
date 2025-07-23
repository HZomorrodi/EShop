using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.ViewModels.Sliders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.EFServices
{
    public class SliderService(IUnitOfWork uow) : GenericService<Slider>(uow), ISliderService
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly DbSet<Slider> _sliders = uow.Set<Slider>();

        public async Task<List<ShowSliderViewModel>> GetPreviewAsync()
            => await _sliders.Select(x => new ShowSliderViewModel()
            {
                FirstTitle = x.FirstTitle,
                SecondTitle = x.SecondTitle,
                Id = x.Id,
                ProductTitle = x.Product.Title
            }).ToListAsync();

        public async Task<EditSliderViewModel?> GetForEdit(int id)
            => await _sliders.Where(x => x.Id == id).Select(x => new EditSliderViewModel()
            {
                Id = x.Id,
                FirstTitle = x.FirstTitle,
                SecondTitle = x.SecondTitle,
                ProductId = x.ProductId
            }).SingleOrDefaultAsync();

        public async Task<List<ShowSliderInFrontViewModel>> GetSlidersForFront()
            => await _sliders.Select(x => new ShowSliderInFrontViewModel()
            {
                ProductId = x.ProductId,
                FirstTitle = x.FirstTitle,
                SecondTitle = x.SecondTitle,
                ProductTitle = x.Product.Title,
                Image = x.Image
            }).ToListAsync();
    }
}
