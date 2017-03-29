using AW.Common.SerializationToolkit;
using AW.Entities.Domain.BaseEntites.Contracts;

namespace AW.Entities.Domain.BaseEntites
{
    [Serializable]
    public class SoftDeletePassivableTitleEntity<TPrimaryKey> : SoftDeleteTitleEntity<TPrimaryKey>, IPassivable
    {
        public bool IsActive { get; set; }
    }

    [Serializable]
    public class SoftDeletePassivableTitleEntity : SoftDeletePassivableTitleEntity<int>
    {
    }
}