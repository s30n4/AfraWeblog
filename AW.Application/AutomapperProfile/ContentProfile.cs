using AutoMapper;
using AW.Application.Dtos.NewsContent;
using AW.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Application.AutomapperProfile
{
    public class ContentProfile: Profile
    {
        public ContentProfile()
        {
            CreateMap<ContentAddDto, NewsContent>()
                .ForMember(dest => dest.SubmitDate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<NewsContent, ContentOutputDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Authors.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.NewsCategories.Name));
        }
    }
}
