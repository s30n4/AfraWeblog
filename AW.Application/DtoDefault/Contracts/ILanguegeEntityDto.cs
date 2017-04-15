using System.ComponentModel.DataAnnotations;

namespace AW.Application.DtoDefault.Contracts
{
    public interface ILanguegeEntityDto
    {
        [Required]
        int LanguageId { get; set; }

        //[MaxLength(150)]
        //string Title { get; set; }

        //[MaxLength(500)]
        //string Description { get; set; }
    }
}