using AutoMapper;
using AW.Application.Dtos.NewsCategory;
using AW.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
