using AutoMapper;
using AW.Application.Dtos.Link;
using AW.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Application.AutomapperProfile
{
    public class LinkProfile: Profile
    {
        public LinkProfile()
        {
            CreateMap<LinkDto, Link>().ReverseMap();
        }
    }
}
