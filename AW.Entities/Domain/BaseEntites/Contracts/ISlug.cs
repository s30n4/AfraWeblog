using System.ComponentModel.DataAnnotations;

namespace AW.Entities.Domain.BaseEntites.Contracts
{
    public interface ISlug
    {
        [MaxLength(150)]
        string Slug { get; set; }
    }
}