using AutoMapper;
using AW.Application.Dtos.Label;
using AW.Entities.Domain;

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
