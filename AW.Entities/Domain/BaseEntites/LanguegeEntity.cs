using System.ComponentModel.DataAnnotations;
using AW.Common.SerializationToolkit;
using AW.Entities.Domain.BaseEntites.Contracts;

namespace AW.Entities.Domain.BaseEntites
{
    [Serializable]
    public abstract class LanguegeEntity : LanguegeEntity<int>
    {
    }

    [Serializable]
    public abstract class LanguegeEntity<TPrimaryKey> : Entity<TPrimaryKey>, IHasLanguege
    {
        [Required]
        public int LangId { get; set; }

        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}