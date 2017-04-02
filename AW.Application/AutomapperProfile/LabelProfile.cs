using AutoMapper;
using AW.Application.Dtos.Label;
using AW.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Application.AutomapperProfile
{
    public class LabelProfile: Profile
    {
        public LabelProfile()
        {
            CreateMap<LabelDto, Label>().ReverseMap();
        }
    }
}
