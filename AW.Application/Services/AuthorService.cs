using AutoMapper;
using AW.Application.Dtos.Author;
using AW.Application.Services.Contracts;
using AW.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Application.Services
{
    public class AuthorService: IAuthor
    {
        
        public int? AddAuthor(AuthorDto data)
        {
           //if (data == null)
                return null;
            //Author aut;
            //if (data.Id == 0)
            //{
            //    aut= Mapper.Map<Author>(data);
                
            //}
            //else
            //{
            //    //aut=d
            //}
            //return aut.Id;
        }

        
    }
}
