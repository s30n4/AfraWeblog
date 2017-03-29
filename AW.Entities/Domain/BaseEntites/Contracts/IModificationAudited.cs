using System;
using System.ComponentModel.DataAnnotations;

namespace AW.Entities.Domain.BaseEntites.Contracts
{
    /// <summary>
    /// This interface is implemented by entities that is wanted to store modification information (who and when modified lastly).
    /// Properties are automatically set when updating the <see cref="IEntity"/>.
    /// </summary>
    public interface IModificationAudited : IHasModificationTime
    {
        /// <summary>
        /// Last modifier user for this entity.
        /// </summary>
        Guid? LastModifierUserId { get; set; }

        [MaxLength(25)]

        string LastModifierUserIp { get; set; }
    }
}