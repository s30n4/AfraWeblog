using System;
using AW.Entities.Domain.BaseEntites.Contracts;

namespace AW.Entities.Domain.BaseEntites
{
    [Serializable]
    public class SoftDeleteTitleHasShowTimeEntity : SoftDeleteTitleHasShowTimeEntity<int>
    {
    }

    [Serializable]
    public abstract class SoftDeleteTitleHasShowTimeEntity<TPrimaryKey> : SoftDeleteTitleEntity<TPrimaryKey>, IHasShowTime
    {
        public DateTime? ShowEndDate { get; set; }

        public DateTime? ShowStartDate { get; set; }
    }
}
