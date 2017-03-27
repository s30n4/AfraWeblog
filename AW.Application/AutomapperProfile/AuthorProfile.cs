using AutoMapper;
using AW.Application.Dtos.Author;
using AW.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
