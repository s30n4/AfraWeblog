using System.ComponentModel.DataAnnotations;
using AW.Application.DtoDefault.Contracts;
using AW.Common.SerializationToolkit; 

namespace AW.Application.DtoDefault
{
    [Serializable]
    public abstract class TitleEntityDto : EntityDto, ITitleEntityDto
    {
        [Required]
        [MaxLength(75)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }

    public abstract class TitleEntityDto<TPrimaryKey> : EntityDto<TPrimaryKey>, ITitleEntityDto
    {
        [Required]
        [MaxLength(75)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }


}