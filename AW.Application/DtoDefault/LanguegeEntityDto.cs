using System.ComponentModel.DataAnnotations;
using AW.Application.DtoDefault.Contracts;
using AW.Common.SerializationToolkit;

namespace AW.Application.DtoDefault
{
    [Serializable]
    public abstract class LanguegeEntityDto : EntityDto, ILanguegeEntityDto
    {
        [Required]
        public int LanguageId { get; set; }

        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}