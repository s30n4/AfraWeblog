using AutoMapper;
using AW.Application.Dtos.Link;
using AW.Entities.Domain;

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
