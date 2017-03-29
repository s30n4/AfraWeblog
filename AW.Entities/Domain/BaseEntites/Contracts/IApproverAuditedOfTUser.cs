using System;

namespace AW.Entities.Domain.BaseEntites.Contracts
{
    public interface IApproverOfTUserAudited : IApproverAudited
    {
        /// <summary>
        /// Reference to the last modifier user of this entity.
        /// </summary>
        Guid? LastApproverUserId { get; set; }

        string LastApproverUserIp { get; set; }
    }
}