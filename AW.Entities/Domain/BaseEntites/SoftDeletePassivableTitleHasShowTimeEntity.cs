
using System;
using AW.Entities.Domain.BaseEntites.Contracts;

namespace AW.Entities.Domain.BaseEntites
{
    [Common.SerializationToolkit.Serializable]
    public abstract class SoftDeletePassivableTitleHasShowTimeEntity : SoftDeletePassivableTitleHasShowTimeEntity<int> { }

    [Common.SerializationToolkit.Serializable]
    public abstract class SoftDeletePassivableTitleHasShowTimeEntity<TPrimaryKey> :
        SoftDeletePassivableTitleEntity<TPrimaryKey>, IHasShowTime
    {
        public DateTime? ShowStartDate { get; set; }
        public DateTime? ShowEndDate { get; set; }
    }
}