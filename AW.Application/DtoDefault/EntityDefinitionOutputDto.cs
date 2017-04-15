using System.ComponentModel.DataAnnotations;

namespace AW.Application.DtoDefault
{
    public class EntityDefinitionOutputDto : EntityDto
    {
        [MaxLength(75)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}