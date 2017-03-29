using System;

namespace AW.Entities.Domain.BaseEntites.Contracts
{
    public interface IHasApproveTime
    {
        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        DateTime? ApproveTime { get; set; }
    }
}