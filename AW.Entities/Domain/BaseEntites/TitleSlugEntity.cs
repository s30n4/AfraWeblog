using System.ComponentModel.DataAnnotations;
using AW.Common.SerializationToolkit;
using AW.Entities.Domain.BaseEntites.Contracts;

namespace AW.Entities.Domain.BaseEntites
{
    [Serializable]
    public abstract class TitleSlugEntity<TPrimaryKey> : TitleEntity<TPrimaryKey>, ISlug
    {
        [MaxLength(150)]
        public string Slug { get; set; }
    }

    [Serializable]
    public abstract class TitleSlugEntity : TitleSlugEntity<int>
    {
    }
}