using EShop.Entities;
using EShop.ViewModels.Sliders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.Contracts
{
    public interface ISliderService : IGenericService<Slider>
    {
        Task<List<ShowSliderViewModel>> GetPreviewAsync();

        Task<EditSliderViewModel?> GetForEdit(int id);

        Task<List<ShowSliderInFrontViewModel>> GetSlidersForFront();
    }
}
