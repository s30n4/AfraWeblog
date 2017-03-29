using System.ComponentModel.DataAnnotations;

namespace AW.Entities.Domain.BaseEntites.Contracts
{
    public interface IHasLanguege
    {
        int LangId { get; set; }

        [MaxLength(150)]
        string Title { get; set; }

        [MaxLength(500)]
        string Description { get; set; }
    }
}