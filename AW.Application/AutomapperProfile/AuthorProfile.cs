using AutoMapper;
using AW.Application.Dtos.Author;
using AW.Entities.Domain;

namespace AW.Application.AutomapperProfile
{
    public class AuthorProfile: Profile
    {
        public AuthorProfile()
        {
            CreateMap<AuthorDto, Author>().ReverseMap();
        }
    }
}
