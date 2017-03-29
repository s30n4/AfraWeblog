using System;
using AW.Entities.Domain.BaseEntites.Contracts;

namespace AW.Entities.Domain.BaseEntites
{
    [Serializable]
    public abstract class HasShowTimeEntity : HasShowTimeEntity<int>
    {
    }

    [Serializable]
    public abstract class HasShowTimeEntity<TPrimaryKey> : Entity<TPrimaryKey>, IHasShowTime
    {
        public DateTime? ShowStartDate { get; set; }
        public DateTime? ShowEndDate { get; set; }
    }
}