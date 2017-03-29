using System;

namespace AW.Entities.Domain.BaseEntites.Contracts
{
    /// <summary>
    /// This interface is implemented by entities which wanted to store deletion information (who and when deleted).
    /// </summary>
    public interface IDeletionAudited : IHasDeletionTime
    {
        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        Guid? DeleterUserId { get; set; }

        string DeleterUserIp { get; set; }
    }
}