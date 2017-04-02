using AutoMapper;
using AW.Application.Dtos.NewsContent;
using AW.Entities.Domain;
using System;

namespace AW.Application.AutomapperProfile
{
    public class ContentProfile: Profile
    {
        public ContentProfile()
        {
            CreateMap<ContentAddDto, NewsContent>();

            CreateMap<NewsContent, ContentOutputDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Authors.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.NewsCategories.Name))
                .ForMember(dest => dest.SubmitDate, opt => opt.MapFrom(src => CommonFunction.ConvertToShamsi(src.SubmitDate)));
        }
    }
}
