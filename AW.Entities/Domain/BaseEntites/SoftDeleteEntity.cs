using System;
using AW.Entities.Domain.BaseEntites.Contracts;

namespace AW.Entities.Domain.BaseEntites
{
    [Serializable]
    public abstract class SoftDeleteEntity : SoftDeleteEntity<int>
    {
    }

    [Serializable]
    public abstract class SoftDeleteEntity<TPrimaryKey> : Entity<TPrimaryKey>, ISoftDelete
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
        public Guid? DeleterUserId { get; set; }
    }
}