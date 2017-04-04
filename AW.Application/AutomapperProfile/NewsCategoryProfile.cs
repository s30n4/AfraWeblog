using AutoMapper;
using AW.Application.Dtos.NewsCategory;
using AW.Entities.Domain;

namespace AW.Application.AutomapperProfile
{
    public class NewsCategoryProfile: Profile
    {
        public NewsCategoryProfile()
        {
            CreateMap<NewsCategoryDto, NewsCategory>().ReverseMap();
        }
    }
}
