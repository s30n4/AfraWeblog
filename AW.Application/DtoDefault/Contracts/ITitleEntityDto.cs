using System.ComponentModel.DataAnnotations;

namespace AW.Application.DtoDefault.Contracts
{
    public interface ITitleEntityDto
    {
        [MaxLength(75)]
        string Name { get; set; }

        [MaxLength(150)]
        string Title { get; set; }

        [MaxLength(500)]
        string Description { get; set; }
    }
}