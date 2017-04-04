using AutoMapper;
using AW.Application.Dtos.Comment;
using AW.Entities.Domain;

namespace AW.Application.AutomapperProfile
{
    public class CommentProfile: Profile
    {
        public CommentProfile()
        {
            CreateMap<CommentAddDto, Comment>();

            CreateMap<Comment, CommentOutputDto>()
                .ForMember(dest=>dest.SubmitDate,opt=>opt.MapFrom(src=> CommonFunction.ConvertToShamsi(src.SubmitDate)));
        }
    }
}
